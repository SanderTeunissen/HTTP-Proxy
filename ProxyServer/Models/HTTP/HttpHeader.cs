namespace ProxyServer.Models.HTTP
{
    public class HttpHeader
    {
        private string _Key;
        public string Key { get => _Key; }

        private string _Value;
        public string Value
        {
            get => _Value;
            set { _Value = value; }
        }

        /// <summary>
        /// Constructor for HTTP header object
        /// </summary>
        /// <param name="key">HTTP header key</param>
        /// <param name="value">HTTP header value</param>
        public HttpHeader(string key, string value)
        {
            _Key = key;
            _Value = value;
        }

        /// <summary>
        /// HTTP header as string
        /// </summary>
        public new string ToString
        {
            get
            {
                return string.Format("{0}: {1}", Key, Value);
            }
        }
    }
}
