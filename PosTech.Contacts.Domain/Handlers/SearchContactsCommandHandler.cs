﻿using AutoMapper;
using MediatR;
using PosTech.Contacts.ApplicationCore.Commands;
using PosTech.Contacts.ApplicationCore.DTOs.Responses;
using PosTech.Contacts.ApplicationCore.Entities;
using PosTech.Contacts.ApplicationCore.Repositories;
using System.Linq.Expressions;

namespace PosTech.Contacts.ApplicationCore.Handlers
{
    public class SearchContactsCommandHandler(IMapper mapper, IContactRepository contactRepository)
        : IRequestHandler<SearchContactsCommand, List<ContactResponseDto>>
    {
        private readonly IMapper _mapper = mapper;
        private readonly IContactRepository _contactRepository = contactRepository;

        public async Task<List<ContactResponseDto>> Handle(SearchContactsCommand request, CancellationToken cancellationToken)
        {
            var filters = CreateFilters(request);
            var expression = CreateExpression(filters);
            var storageContacts = await _contactRepository.FindContactsAsync(expression);
            var contacts = _mapper.Map<List<ContactResponseDto>>(storageContacts);

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
