﻿using AutoMapper;
using MediatR;
using PosTech.Contacts.ApplicationCore.Commands;
using PosTech.Contacts.ApplicationCore.DTOs.Responses;
using PosTech.Contacts.ApplicationCore.Entities;
using PosTech.Contacts.ApplicationCore.Repositories;
using PosTech.Contacts.ApplicationCore.Serialization;
using PosTech.Contacts.ApplicationCore.Services;
using Serilog;
using System.Linq.Expressions;

namespace PosTech.Contacts.ApplicationCore.Handlers
{
    public class SearchContactsCommandHandler(ICacheService cacheService, IContactRepository contactRepository, IMapper mapper)
        : IRequestHandler<SearchContactsCommand, List<ContactResponseDto>>
    {
        private readonly ICacheService _cacheService = cacheService;
        private readonly IContactRepository _contactRepository = contactRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<List<ContactResponseDto>> Handle(SearchContactsCommand request, CancellationToken cancellationToken)
        {
            var key = JsonSerializerHelper.Serialize(request);

            Log.Information("Iniciando busca de contatos com base nos filtros informados.");

            if (_cacheService.TryGetValue<List<ContactResponseDto>>(key, out var contacts))
            {
                Log.Information("Retornando contatos armazenados no cache.");

                return contacts;
            }

            var filters = CreateFilters(request);
            var expression = CreateExpression(filters);            
            var storageContacts = await _contactRepository.FindContactsAsync(expression);

            contacts = _mapper.Map<List<ContactResponseDto>>(storageContacts);

            Log.Information("Armazenando contatos no cache");

            await _cacheService.SetAsync(key, contacts, TimeSpan.FromMinutes(10));

            Log.Information("Busca de contatos finalizada.");

            return contacts;
        }

        private static List<Filter> CreateFilters(SearchContactsCommand request)
        {
            var filters = new List<Filter>();

            var type = request.GetType();
            var properties = type.GetProperties();

            foreach (var property in properties)
            {
                if (property.GetValue(request) is not null && !string.IsNullOrEmpty(property.GetValue(request).ToString()))
                    filters.Add(new Filter { ColumnName = property.Name, Value = property.GetValue(request).ToString() });
            }

            return filters;
        }

        private Expression<Func<Contact, bool>> CreateExpression(List<Filter> filters)
        {
            var parameter = Expression.Parameter(typeof(Contact), nameof(Contact));
            Expression filterExpression = null;

            foreach (var filter in filters)
            {
                var property = Expression.Property(parameter, filter.ColumnName);
                var constant = Expression.Constant(filter.Value);
                Expression comparison;

                if (property.Type == typeof(string))
                {
                    comparison = Expression.Call(property, "Contains", Type.EmptyTypes, constant);
                }
                else
                {
                    constant = Expression.Constant(Convert.ToInt32(filter.Value));
                    comparison = Expression.Equal(property, constant);
                }

                filterExpression = filterExpression == null ? comparison : Expression.And(filterExpression, comparison);
            }

            var expression = Expression.Lambda<Func<Contact, bool>>(filterExpression, parameter);

            return expression;
        }
    }

    class Filter
    {
        public string ColumnName { get; set; }
        public string Value { get; set; }
    }
}
