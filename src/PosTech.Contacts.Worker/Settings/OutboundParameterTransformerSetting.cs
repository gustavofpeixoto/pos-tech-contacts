using System.Text.RegularExpressions;

namespace PosTech.Contacts.Worker.Settings
{
    public partial class OutboundParameterTransformerSetting : IOutboundParameterTransformer
    {
        public string TransformOutbound(object value)
        {
            if (value == null) return null;

            return ControllerNameRegex().Replace(value.ToString(), "$1-$2").ToLower();
        }

        [GeneratedRegex("([a-z])([A-Z])")]
        private static partial Regex ControllerNameRegex();
    }
}
