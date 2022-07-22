using System.Text.Json.Serialization;

namespace MyJetWallet.Sdk.WalletApi.Contracts
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ApiResponseCodes
    {
        OK = 0,
        InternalServerError = 1,        // Something went wrong. Please try again later.
        WalletDoNotExist = 2,           // Wallet is not found.
        LowBalance = 3,                 // ... depo, trade...
        CannotProcessWithdrawal = 4,    // Withdrawal request failed. Please try again later.
        AddressIsNotValid = 5,          // Invalid address. // (withdraw address verification)
        AssetDoNotFound = 6,            // Asset is not found.
        AssetIsDisabled = 7,            // Asset is not supported / Trading is not available for this asset now. Please try again later.
        AmountIsSmall = 8,              // Your amount is too small.
        InvalidInstrument = 9,          // Asset is not supported.
        KycNotPassed = 10,              // Your account is not verified. Complete KYC verification now.
        AssetDoNotSupported = 11,       // Asset is not supported / Trading is not available for this asset now. Please try again later.
        NotEnoughLiquidityForMarketOrder = 12,   // 
        InvalidOrderValue = 13,                  // 
        CannotProcessQuoteRequest = 14,          // 
        CannotExecuteQuoteRequest = 15,          // 
        NoqEnoughLiquidityForConvert = 16,       // 
        LeadToNegativeSpread = 17,               // 
        WithdrawalDoNotFound = 18,               // 
        AddressDoNotSupported = 19,              // 
        CannotResendWithdrawalVerification = 20, //
        PhoneIsNotConfirmed = 21,                //
        NotSupported = 22,                       //
        OperationNotFound = 23,                  //
        OperationNotAllowed = 24,                //
        BlockchainIsNotConfigured = 25,          //
        BlockchainIsNotSupported = 26,           //
        InvalidPhone = 27,
        AmountToLarge = 28,                      // Amount to large
        OperationUnavailable = 29, 
        InvalidDeletionReasons = 30,
        InvalidSwapPair = 31,
        AssetIsNotConfigued = 32,

        //Auth
        InvalidUserNameOrPassword = 101,      // Invalid login or password.
        UserExists = 102,                     // ??а нужно ли? мы же сделаи обходной флоу
        UserNotExist = 103,                   // ?? User not found
        OldPasswordNotMatch = 104,            // You entered the wrong current password
        Expired = 105,                        // Session has expired. Please log in again
        CountryIsRestricted = 106,            // Registration from your country is not allowed
        SocialNetworkNotSupported = 107,      // Social network is not available for log in
        SocialNetworkNotAvailable = 108,      // Social network is not available for log in
        ValidationError = 109,                // 
        BrandNotFound = 110,                  // Something went wrong. Please try again
        InvalidToken = 111,                   // ?? Invalid token. Please log in again
        RecaptchaFailed = 112,                // The CAPTCHA verification failed. Please try again
        InvalidCode = 113,                    //  
        InvalidRefCode = 114,  
        
        //Circle, Cards
        InvalidKeyId = 201,                     // Invalid key id ????
        InvalidEncryptedData = 202,             // ????
        InvalidBillingName = 203,               // Invalid Name
        InvalidBillingCity = 204,               // Invalid city
        InvalidBillingCountry = 205,            // Invalid country
        InvalidBillingLine1 = 206,              // Invalid ???
        InvalidBillingDistrict = 207,           // Invalid district
        InvalidBillingPostalCode = 208,         // Invalid postal code
        InvalidExpMonth = 209,                  // Invalid expiration month
        InvalidExpYear = 210,                   // Invalid expiration year
        CardAddressMismatch = 211,              // ??
        CardZipMismatch = 212,                  // ??
        CardCvvInvalid = 213,                   // Invalid CVV code
        CardExpired = 214,                      // Card is expired
        CardFailed = 215,                       // 
        CardNotFound = 216,                     // 
        PaymentFailed = 217,                    // 
        CardFirstAndLastNameCannotBeEmpty = 218,  // 
        InvalidGuid = 219, // 

        InvalidAccountNumber = 220, //  
        InvalidRoutingNumber = 221, //  
        InvalidIban = 222, //  
        BankAccountNotFound = 223, //  
        ExistingRequestId = 224, //
        OperationBlocked = 225, //
        InvalidCardNumber = 226,
        DisclaimerConfirmRequired = 227,
        
        // Card extension codes
        VerificationFailed = 228,
        VerificationNotSupportedByIssuer = 229,
        CardInvalid = 230,
        CardNotHonored = 231,
        CardAccountIneligible = 232,
        CardLimitViolated = 233,
        CardCvvRequired = 234,
        ThreeDSecureNotSupported = 235,
        ThreeDSecureActionExpired = 236,
        ThreeDSecureInvalidRequest = 237,
        
        PaymentNotSupportedByIssuer = 238,
        PaymentNotFunded = 239,
        PaymentFailedBalanceCheck = 240,
        //CardInvalid = 230,
        //CardLimitViolated = 233,
        //CardNotHonored = 231,
        CardRestricted = 241,
        
        // Bank extension codes
        BankAccountIneligible = 242,
        //InvalidAccountNumber = 220,
        InvalidWireRtn = 243,
        RefIdInvalid = 244,
        AccountNameMismatch = 245,
        AccountNumberMismatch = 246,
        AccountIneligible = 247,
        WalletAddressMismatch = 248,
        CustomerNameMismatch = 249,
        InstitutionNameMismatch = 250,
        TransferFailed = 251,
        InsufficientFunds = 252,
        
        //Payout
        //TODO: Implement codes return to WithdrawalProcessor func ExecuteCirclePayoutsAsync()
        //InsufficientFunds = 252,
        TransactionDenied = 253,
        TransactionFailed = 254,
        TransactionReturned = 255,
        BankTransactionError = 256,
        FiatAccountLimitExceeded = 257,
        InvalidBankAccountNumber = 258,
        InvalidAchRtn = 259,
        //InvalidWireRtn = 243,
        
        //High-Yield
        ClientOfferDisabled = 303,
        OfferDisabled = 305,
        ClientOfferFinished = 306,
        TierNotFound = 307,
        CannotChangeBalance = 308,
        TopUpBlocked = 309,
        ConvertAssetError = 310,
        ClientOfferNotFound = 311
    }
}
