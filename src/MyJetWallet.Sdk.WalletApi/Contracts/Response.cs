namespace MyJetWallet.Sdk.WalletApi.Contracts
{
    public class Response
    {
        public ApiResponseCodes Result { get; set; }
        
        public UnauthorizedData RejectDetail { get; set; }

        public Response(ApiResponseCodes result)
        {
            Result = result;
        }

        public static Response OK()
        {
            return new Response(ApiResponseCodes.OK);
        }
        
        public static Response RejectWithDetails(ApiResponseCodes code, UnauthorizedData detail)
        {
            return new Response(code)
            {
                RejectDetail = detail
            };
        }
    }

    public class Response<T> : Response where T: class
    {
        public T Data { get; set; }

        public Response(T data) : base(ApiResponseCodes.OK)
        {
            Data = data;
        }
        
        public Response(ApiResponseCodes code, T data) : base(code)
        {
            Data = data;
        }

        public Response(ApiResponseCodes result) : base(result)
        {
            Data = null;
        }
    }
}