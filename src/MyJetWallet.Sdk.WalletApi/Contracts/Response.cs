namespace MyJetWallet.Sdk.WalletApi.Contracts
{
    public class Response
    {
        public ApiResponseCodes Result { get; set; }
        
        public UnauthorizedData RejectDetail { get; set; }
        public string Message { get; set; }

        public Response(ApiResponseCodes result, string message)
        {
            Result = result;
            Message = message;
        }

        public static Response OK()
        {
            return new Response(ApiResponseCodes.OK, "OK");
        }
        
        public static Response RejectWithDetails(ApiResponseCodes code, string message, UnauthorizedData detail)
        {
            return new Response(code, message)
            {
                RejectDetail = detail
            };
        }
    }

    public class Response<T> : Response where T: class
    {
        public T Data { get; set; }

        public Response(T data) : base(ApiResponseCodes.OK, "OK")
        {
            Data = data;
        }
        
        public Response(ApiResponseCodes code, string message, T data) : base(code, message)
        {
            Data = data;
        }

        public Response(ApiResponseCodes result, string message) : base(result, message)
        {
            Data = null;
        }
    }
}