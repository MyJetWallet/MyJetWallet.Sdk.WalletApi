using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace MyJetWallet.Sdk.WalletApi.Contracts
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ApiResponseCodes
    {
        OK = 0,
        InternalServerError = 1, // Something went wrong. Please try again later.
        WalletDoNotExist = 2, // Wallet is not found.
        LowBalance = 3, // ... depo, trade...
        CannotProcessWithdrawal = 4, // Withdrawal request failed. Please try again later.
        AddressIsNotValid = 5, // Invalid address. // (withdraw address verification)
        AssetDoNotFound = 6, // Asset is not found.

        AssetIsDisabled = 7, // Asset is not supported / Trading is not available for this asset now. Please try again later.
        AmountIsSmall = 8, // Your amount is too small.
        InvalidInstrument = 9, // Asset is not supported.
        KycNotPassed = 10, // Your account is not verified. Complete KYC verification now.

        AssetDoNotSupported = 11, // Asset is not supported / Trading is not available for this asset now. Please try again later.
        //NotEnoughLiquidityForMarketOrder = 12, // 
        //InvalidOrderValue = 13, // 
        CannotProcessQuoteRequest = 14, // 
        CannotExecuteQuoteRequest = 15, // 
        NoqEnoughLiquidityForConvert = 16, // 
        //LeadToNegativeSpread = 17, // 
        WithdrawalDoNotFound = 18, // 
        //AddressDoNotSupported = 19, // 
        CannotResendWithdrawalVerification = 20, //
        PhoneIsNotConfirmed = 21, //
        NotSupported = 22, //
        OperationNotFound = 23, //
        OperationNotAllowed = 24, //
        BlockchainIsNotConfigured = 25, //
        BlockchainIsNotSupported = 26, //
        InvalidPhone = 27,
        AmountToLarge = 28, // Amount to large
        OperationUnavailable = 29,
        InvalidDeletionReasons = 30,
        InvalidSwapPair = 31,
        AssetIsNotConfigued = 32,
        ClientMustBeOlder = 33,
        ProfileAlreadyExist = 34,
        AnotherVerificationInProgress = 35,
        DocumentsNotUploaded = 36,
        WithdrawalBlocked = 37,
        TransferBlocked = 38,
        TradeBlocked = 39, 
        DepositBlocked = 40,
        LoginBlocked = 41,
        PhoneNumberUpdateBlocked = 42,
        BlockchainSendNotSupported = 43,
        InternalSendNotSupported = 44,
        BlockchainReceiveNotSupported = 45,
        LimitExceeded = 46, // personal limit by payment method is hit
        TradingDisabled = 47, // disable trading on the system
        InvalidInvoice = 48, // invalid invoice
        LimitReached = 49, // limit reached
        ServiceUnavailable = 50, // service unavailable
        InvalidEmail = 51,
        BalanceTooBigForDeletion = 52,
        AmountNotEnoughToPayFee = 53,
        TransfersToBankingBlocked = 54,
        BankingOperationsBlocked = 55,
        AddressGenerationInProgress = 56,
        JarNotFound = 57,
        JarWithdrawalLimitExceeded = 58,
        JarCountLimitExceeded = 59,
        EmailUnavailable = 60,
        UsdtEurNotSupported = 61,
        UsdtNotSupported = 62,
        
        //Auth
        //InvalidUserNameOrPassword = 101, // Invalid login or password.
        UserExists = 102, // ??а нужно ли? мы же сделаи обходной флоу
        //UserNotExist = 103, // ?? User not found
        //OldPasswordNotMatch = 104, // You entered the wrong current password
        //Expired = 105, // Session has expired. Please log in again
        CountryIsRestricted = 106, // Registration from your country is not allowed
        //SocialNetworkNotSupported = 107, // Social network is not available for log in
        //SocialNetworkNotAvailable = 108, // Social network is not available for log in
        ValidationError = 109, // 
        BrandNotFound = 110, // Something went wrong. Please try again
        InvalidToken = 111, // ?? Invalid token. Please log in again
        //RecaptchaFailed = 112, // The CAPTCHA verification failed. Please try again
        InvalidCode = 113, //  
        InvalidRefCode = 114,
        PinCodeAlreadyExist = 115, // user can't setup new pin, because pin already exist
        SelfieNotExist = 116, // user can't setup new pin, because pin already exist
        WrongPinCodeAttempt = 117, // wrong pin code with attempts
        WrongPinCodeBlocked = 118, // wrong pin code hit block time
        WebLoginTemporallyNotAvailable = 119, // wrong pin code hit block time


        //Circle, Cards
        InvalidKeyId = 201, // Invalid key id ????
        InvalidEncryptedData = 202, // ????
        InvalidBillingName = 203, // Invalid Name
        InvalidBillingCity = 204, // Invalid city
        InvalidBillingCountry = 205, // Invalid country
        InvalidBillingLine1 = 206, // Invalid ???
        InvalidBillingDistrict = 207, // Invalid district
        InvalidBillingPostalCode = 208, // Invalid postal code
        InvalidExpMonth = 209, // Invalid expiration month
        InvalidExpYear = 210, // Invalid expiration year
        CardAddressMismatch = 211, // ??
        CardZipMismatch = 212, // ??
        CardCvvInvalid = 213, // Invalid CVV code
        CardExpired = 214, // Card is expired
        CardFailed = 215, // 
        CardNotFound = 216, // 
        PaymentFailed = 217, // 
        CardFirstAndLastNameCannotBeEmpty = 218, // 
        //InvalidGuid = 219, //

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
        PaymentStoppedByIssuer = 260,
        PaymentCanceled = 261,
        PaymentReturned = 262,
        CreditCardNotAllowed = 263,
        PaymentMethodUnsupported = 264,
        VerificationDenied = 265,
        ThreeDSecureRequired = 266,
        ThreeDSecureFailure = 267,
        PaymentFailedDueToPaymentProcessorError = 268,
        DeclineByBank = 269,
        
        //Wrong Card Country
        CardCountryNotSupported = 270,
        CardCountryNotSupportedExceptVisa = 271,
        CardTypeNotSupported = 272,

        CardBlocked = 280,
        TooManyCardsIssued = 281,
        
        //High-Yield
        ClientOfferDisabled = 303,
        OfferDisabled = 305,
        ClientOfferFinished = 306,
        TierNotFound = 307,
        CannotChangeBalance = 308,
        TopUpBlocked = 309,
        ConvertAssetError = 310,
        ClientOfferNotFound = 311,

        //NFT
        NftIsNotOnSale = 400,

        //Fee
        NotEnoughBalanceToCoverFee = 500,


        //Nickname
        InvalidNickname = 600,
        NicknameUsed = 601,
        NoNicknameChangesLeft = 602,
        ContactWithThisIbanAlreadyExists = 603,
        ContactWithThisNicknameAlreadyExists = 604,
        ContactWithThisNameAlreadyExists = 605,
        ContactNotFound = 606,
        DepositAssetNotAvailable = 607,
        
        //Verification Api
        UnsuccessfulSend = 701,
        PhoneNotFound = 702,
        LanguageNotSet = 703,
        PhoneDuplicate = 704,
        WrongPinWhenChangingPhone = 705,
        PhoneBindingRequired = 706,
        
        //Iban pt.2
        AddressInvalidOnlyLatinSymbolAllowed = 801,
        IbanNotReachable = 802,
        CardNumberRequired = 803,
        IbanRequired = 804,
        PhoneNumberRequired = 805,
        RecipientNameRequired = 806,
        PanNumberRequired = 807,
        UpiAddressRequired = 808,
        AccountNumberRequired = 809,
        BeneficiaryNameRequired = 810,
        BankNameRequired = 811,
        IfscCodeRequired = 812,
        BankAccountRequired = 813,
        AccountLabelAlreadyUsed = 814,
        InvalidBic = 815,
        TinRequired = 816,

        //Gifts
        SimpleKycRequired = 901,
        FullKycRequired = 902,
        GiftNotFound = 903,
        GiftAlreadyClaimed = 904,
        GiftExpired = 905,
        GiftCancelled = 906,

        //Rewards
        NotEnoughSpins = 1001,
        RewardBalanceTooLow = 1002,
        
        // invest reject codes 10_000 - 19_999
        InvestInstrumentNotFound = 10_000,
        InvestAssetNotFound = 10_001,
        InvestMultiplicatorToSmall = 10_002,
        InvestMultiplicatorToBig = 10_003,
        InvestVolumeToSmall = 10_004,
        InvestVolumeToBig = 10_005,
        InvestPendingPriceToSmall = 10_006,
        InvestPendingPriceToBig = 10_007,
        InvestPositionNotFount = 10_008,
        InvestOperationNotAvailable = 10_009,
        InvestOrderCannotReplace = 10_010,
        
        // Earn Engine reject codes 20_000 - 20_099
        EarnOfferNotFound = 20_000,
        EarnPositionNotFound = 20_001,
        
        
        EarnLastCode = 20_099,
        
        // Trading codes
        TradingPositionInvalidOrder = 30_000,
        TradingPositionInsufficientMargin = 30_001,
        TradingLowBalance = 30_002,
        TradingAgentAlreadyExists = 30_003,
        TradingUnknownError = 30_099,
        
        //Embedded 100_000+
        //Transaction 100_000 - 100_999
        OperationNotSupported = 100_000,
        LowBalanceForTransaction = 100_001,
        LowDestinationBalance = 100_002,
        InvalidPeerType = 100_003,
        TransactionNotFound = 100_004,
        OperationNotAvailable = 100_005,
        DestinationWalletNotFound = 100_006,
        DestinationAssetNotFound = 100_007,
        DestinationAddressIsSuspended = 100_008,
        DestinationAddressIsNotValid = 100_009,
        MissingTag = 100_010,
        InvalidSourceType = 100_011,
        InvalidDestination = 100_012,
        InvalidAmount = 100_013,
        InvalidTag = 100_014,
        UnsupportedBlockchain = 100_015,
        FeeTooLow = 100_016,
        InvalidAsset = 100_017,
        UnableToEstimateFee = 100_018,
        PresetsNotFound = 100_019,
        AddressNotFound = 100_020,
        AddressIsNotUnique = 100_021,
        AddressDuplicate = 100_022,
        GaslessNotAllowedForAsset = 100_023,

        //NOTE: добавил код - добавь парамтеры для шаблона, даже если они пустые


        //200_000 to 210_000 HyperLiquid Error Codes
        NotEnoughMargin = 200_000,
        
    }

    public static class ApiResponseClassData
    {
        static ApiResponseClassData()
        {
            AddBody(ApiResponseCodes.OK);
            AddBody(ApiResponseCodes.InternalServerError);
            AddBody(ApiResponseCodes.WalletDoNotExist);
            AddBody(ApiResponseCodes.LowBalance);
            AddBody(ApiResponseCodes.CannotProcessWithdrawal);
            AddBody(ApiResponseCodes.AddressIsNotValid);
            AddBody(ApiResponseCodes.AssetDoNotFound);
            AddBody(ApiResponseCodes.AssetIsDisabled);
            AddBody(ApiResponseCodes.AmountIsSmall, "MINAMOUNT");
            AddBody(ApiResponseCodes.InvalidInstrument);
            AddBody(ApiResponseCodes.KycNotPassed);
            AddBody(ApiResponseCodes.AssetDoNotSupported);
            AddBody(ApiResponseCodes.CannotProcessQuoteRequest);
            AddBody(ApiResponseCodes.CannotExecuteQuoteRequest);
            AddBody(ApiResponseCodes.NoqEnoughLiquidityForConvert);
            AddBody(ApiResponseCodes.WithdrawalDoNotFound);
            AddBody(ApiResponseCodes.CannotResendWithdrawalVerification);
            AddBody(ApiResponseCodes.PhoneIsNotConfirmed);
            AddBody(ApiResponseCodes.OperationNotFound);
            AddBody(ApiResponseCodes.OperationNotAllowed);
            AddBody(ApiResponseCodes.BlockchainIsNotConfigured);
            AddBody(ApiResponseCodes.BlockchainIsNotSupported);
            AddBody(ApiResponseCodes.InvalidPhone);
            AddBody(ApiResponseCodes.AmountToLarge, "MAXAMOUNT");
            AddBody(ApiResponseCodes.OperationUnavailable);
            AddBody(ApiResponseCodes.InvalidDeletionReasons);
            AddBody(ApiResponseCodes.InvalidSwapPair);
            AddBody(ApiResponseCodes.AssetIsNotConfigued);
            AddBody(ApiResponseCodes.ClientMustBeOlder);
            AddBody(ApiResponseCodes.ProfileAlreadyExist);
            AddBody(ApiResponseCodes.AnotherVerificationInProgress);
            AddBody(ApiResponseCodes.DocumentsNotUploaded);
            AddBody(ApiResponseCodes.CountryIsRestricted);
            AddBody(ApiResponseCodes.ValidationError);
            AddBody(ApiResponseCodes.BrandNotFound);
            AddBody(ApiResponseCodes.InvalidToken);
            AddBody(ApiResponseCodes.InvalidCode);
            AddBody(ApiResponseCodes.InvalidRefCode);
            AddBody(ApiResponseCodes.PinCodeAlreadyExist);
            AddBody(ApiResponseCodes.SelfieNotExist);
            AddBody(ApiResponseCodes.InvalidKeyId);
            AddBody(ApiResponseCodes.InvalidEncryptedData);
            AddBody(ApiResponseCodes.InvalidBillingName);
            AddBody(ApiResponseCodes.InvalidBillingCity);
            AddBody(ApiResponseCodes.InvalidBillingCountry);
            AddBody(ApiResponseCodes.InvalidBillingLine1);
            AddBody(ApiResponseCodes.InvalidBillingDistrict);
            AddBody(ApiResponseCodes.InvalidBillingPostalCode);
            AddBody(ApiResponseCodes.InvalidExpMonth);
            AddBody(ApiResponseCodes.InvalidExpYear);
            AddBody(ApiResponseCodes.CardAddressMismatch);
            AddBody(ApiResponseCodes.CardZipMismatch);
            AddBody(ApiResponseCodes.CardCvvInvalid);
            AddBody(ApiResponseCodes.CardExpired);
            AddBody(ApiResponseCodes.CardFailed);
            AddBody(ApiResponseCodes.CardNotFound);
            AddBody(ApiResponseCodes.PaymentFailed);
            AddBody(ApiResponseCodes.CardFirstAndLastNameCannotBeEmpty);
            AddBody(ApiResponseCodes.CardBlocked);
            AddBody(ApiResponseCodes.InvalidAccountNumber);
            AddBody(ApiResponseCodes.InvalidRoutingNumber);
            AddBody(ApiResponseCodes.InvalidIban);
            AddBody(ApiResponseCodes.BankAccountNotFound);
            AddBody(ApiResponseCodes.ExistingRequestId);
            AddBody(ApiResponseCodes.OperationBlocked);
            AddBody(ApiResponseCodes.InvalidCardNumber);
            AddBody(ApiResponseCodes.DisclaimerConfirmRequired);
            AddBody(ApiResponseCodes.VerificationFailed);
            AddBody(ApiResponseCodes.VerificationNotSupportedByIssuer);
            AddBody(ApiResponseCodes.CardInvalid);
            AddBody(ApiResponseCodes.CardNotHonored);
            AddBody(ApiResponseCodes.CardAccountIneligible);
            AddBody(ApiResponseCodes.CardLimitViolated);
            AddBody(ApiResponseCodes.CardCvvRequired);
            AddBody(ApiResponseCodes.ThreeDSecureNotSupported);
            AddBody(ApiResponseCodes.ThreeDSecureActionExpired);
            AddBody(ApiResponseCodes.ThreeDSecureInvalidRequest);
            AddBody(ApiResponseCodes.PaymentNotSupportedByIssuer);
            AddBody(ApiResponseCodes.PaymentNotFunded);
            AddBody(ApiResponseCodes.PaymentFailedBalanceCheck);
            AddBody(ApiResponseCodes.CardRestricted);
            AddBody(ApiResponseCodes.BankAccountIneligible);
            AddBody(ApiResponseCodes.InvalidWireRtn);
            AddBody(ApiResponseCodes.RefIdInvalid);
            AddBody(ApiResponseCodes.AccountNameMismatch);
            AddBody(ApiResponseCodes.AccountNumberMismatch);
            AddBody(ApiResponseCodes.AccountIneligible);
            AddBody(ApiResponseCodes.WalletAddressMismatch);
            AddBody(ApiResponseCodes.CustomerNameMismatch);
            AddBody(ApiResponseCodes.InstitutionNameMismatch);
            AddBody(ApiResponseCodes.TransferFailed);
            AddBody(ApiResponseCodes.InsufficientFunds);
            AddBody(ApiResponseCodes.TransactionDenied);
            AddBody(ApiResponseCodes.TransactionFailed);
            AddBody(ApiResponseCodes.TransactionReturned);
            AddBody(ApiResponseCodes.BankTransactionError);
            AddBody(ApiResponseCodes.FiatAccountLimitExceeded);
            AddBody(ApiResponseCodes.InvalidBankAccountNumber);
            AddBody(ApiResponseCodes.InvalidAchRtn);
            AddBody(ApiResponseCodes.PaymentStoppedByIssuer);
            AddBody(ApiResponseCodes.PaymentCanceled);
            AddBody(ApiResponseCodes.PaymentReturned);
            AddBody(ApiResponseCodes.CreditCardNotAllowed);
            AddBody(ApiResponseCodes.PaymentMethodUnsupported);
            AddBody(ApiResponseCodes.VerificationDenied);
            AddBody(ApiResponseCodes.ThreeDSecureRequired);
            AddBody(ApiResponseCodes.ThreeDSecureFailure);
            AddBody(ApiResponseCodes.PaymentFailedDueToPaymentProcessorError);
            AddBody(ApiResponseCodes.DeclineByBank);
            AddBody(ApiResponseCodes.CardCountryNotSupported);
            AddBody(ApiResponseCodes.CardCountryNotSupportedExceptVisa);
            AddBody(ApiResponseCodes.CardTypeNotSupported);
            AddBody(ApiResponseCodes.ClientOfferDisabled);
            AddBody(ApiResponseCodes.OfferDisabled);
            AddBody(ApiResponseCodes.ClientOfferFinished);
            AddBody(ApiResponseCodes.TierNotFound);
            AddBody(ApiResponseCodes.CannotChangeBalance);
            AddBody(ApiResponseCodes.TopUpBlocked);
            AddBody(ApiResponseCodes.ConvertAssetError);
            AddBody(ApiResponseCodes.ClientOfferNotFound);
            AddBody(ApiResponseCodes.NftIsNotOnSale);
            AddBody(ApiResponseCodes.NotEnoughBalanceToCoverFee);
            AddBody(ApiResponseCodes.InvalidNickname);
            AddBody(ApiResponseCodes.NicknameUsed);
            AddBody(ApiResponseCodes.NoNicknameChangesLeft);
            AddBody(ApiResponseCodes.UnsuccessfulSend);
            AddBody(ApiResponseCodes.PhoneNotFound);
            AddBody(ApiResponseCodes.PhoneDuplicate, "OWNEREMAIL");
            AddBody(ApiResponseCodes.LanguageNotSet);
            AddBody(ApiResponseCodes.WrongPinWhenChangingPhone);
            AddBody(ApiResponseCodes.DepositBlocked, "EXPIRETIME");
            AddBody(ApiResponseCodes.LoginBlocked, "EXPIRETIME");
            AddBody(ApiResponseCodes.TradeBlocked, "EXPIRETIME");
            AddBody(ApiResponseCodes.TransferBlocked, "EXPIRETIME");
            AddBody(ApiResponseCodes.WithdrawalBlocked, "EXPIRETIME");
            AddBody(ApiResponseCodes.PhoneNumberUpdateBlocked, "EXPIRETIME");
            AddBody(ApiResponseCodes.WebLoginTemporallyNotAvailable, "EXPIRETIME");
            AddBody(ApiResponseCodes.NotSupported);
            AddBody(ApiResponseCodes.UserExists);
            AddBody(ApiResponseCodes.BlockchainSendNotSupported);
            AddBody(ApiResponseCodes.InternalSendNotSupported);
            AddBody(ApiResponseCodes.BlockchainReceiveNotSupported);
            AddBody(ApiResponseCodes.LimitExceeded, "LIMIT");
            AddBody(ApiResponseCodes.TradingDisabled);
            AddBody(ApiResponseCodes.WrongPinCodeAttempt, "ATTEMPTS_LEFT");
            AddBody(ApiResponseCodes.WrongPinCodeBlocked, "BLOCK_TIME_LEFT_MIN", "BLOCK_TIME_LEFT_SEC");
            AddBody(ApiResponseCodes.ContactWithThisIbanAlreadyExists);
            AddBody(ApiResponseCodes.ContactWithThisNameAlreadyExists);
            AddBody(ApiResponseCodes.ContactWithThisNicknameAlreadyExists);
            AddBody(ApiResponseCodes.ContactNotFound);
            AddBody(ApiResponseCodes.DepositAssetNotAvailable);
            AddBody(ApiResponseCodes.AddressInvalidOnlyLatinSymbolAllowed, "INVALID_FIELD");
            AddBody(ApiResponseCodes.IbanNotReachable);
            AddBody(ApiResponseCodes.CardNumberRequired);
            AddBody(ApiResponseCodes.IbanRequired);
            AddBody(ApiResponseCodes.PhoneNumberRequired);
            AddBody(ApiResponseCodes.RecipientNameRequired);
            AddBody(ApiResponseCodes.PanNumberRequired);
            AddBody(ApiResponseCodes.UpiAddressRequired);
            AddBody(ApiResponseCodes.AccountNumberRequired);
            AddBody(ApiResponseCodes.BeneficiaryNameRequired);
            AddBody(ApiResponseCodes.BankNameRequired);
            AddBody(ApiResponseCodes.IfscCodeRequired);
            AddBody(ApiResponseCodes.BankAccountRequired);
            AddBody(ApiResponseCodes.InvalidInvoice);
            AddBody(ApiResponseCodes.LimitReached);
            AddBody(ApiResponseCodes.ServiceUnavailable);
            AddBody(ApiResponseCodes.SimpleKycRequired);
            AddBody(ApiResponseCodes.FullKycRequired);
            AddBody(ApiResponseCodes.GiftExpired);
            AddBody(ApiResponseCodes.GiftCancelled);
            AddBody(ApiResponseCodes.GiftNotFound);
            AddBody(ApiResponseCodes.GiftAlreadyClaimed);
            AddBody(ApiResponseCodes.InvalidEmail);
            AddBody(ApiResponseCodes.NotEnoughSpins);
            AddBody(ApiResponseCodes.RewardBalanceTooLow);
            
            // invest reject codes 10_000 - 20_000
            AddBody(ApiResponseCodes.InvestInstrumentNotFound);
            AddBody(ApiResponseCodes.InvestAssetNotFound);
            AddBody(ApiResponseCodes.InvestMultiplicatorToSmall, "MIN_MULTIPLICATOR");
            AddBody(ApiResponseCodes.InvestMultiplicatorToBig, "MAX_MULTIPLICATOR");
            AddBody(ApiResponseCodes.InvestVolumeToSmall, "MIN_VOLUME");
            AddBody(ApiResponseCodes.InvestVolumeToBig, "MAX_VOLUME");
            AddBody(ApiResponseCodes.InvestPendingPriceToSmall, "MIN_PRICE");
            AddBody(ApiResponseCodes.InvestPendingPriceToBig,"MAX_PRICE");
            AddBody(ApiResponseCodes.InvestPositionNotFount);
            AddBody(ApiResponseCodes.InvestOperationNotAvailable);
            AddBody(ApiResponseCodes.InvestOrderCannotReplace);
            
            // Earn Engine reject codes 20_000 - 20_099
            AddBody(ApiResponseCodes.EarnOfferNotFound);
            AddBody(ApiResponseCodes.EarnPositionNotFound);
            AddBody(ApiResponseCodes.EarnLastCode);

            AddBody(ApiResponseCodes.PhoneBindingRequired);
            AddBody(ApiResponseCodes.AccountLabelAlreadyUsed);
            AddBody(ApiResponseCodes.InvalidBic);
            AddBody(ApiResponseCodes.BalanceTooBigForDeletion);
            AddBody(ApiResponseCodes.AmountNotEnoughToPayFee);
            AddBody(ApiResponseCodes.TransfersToBankingBlocked);
            AddBody(ApiResponseCodes.BankingOperationsBlocked);
            AddBody(ApiResponseCodes.AddressGenerationInProgress);
            AddBody(ApiResponseCodes.JarNotFound);
            AddBody(ApiResponseCodes.JarWithdrawalLimitExceeded, "LIMIT", "AMOUNT", "LEFT");
            AddBody(ApiResponseCodes.JarCountLimitExceeded);
            AddBody(ApiResponseCodes.EmailUnavailable);
            AddBody(ApiResponseCodes.TinRequired);
            AddBody(ApiResponseCodes.TooManyCardsIssued);
            
            AddBody(ApiResponseCodes.UsdtEurNotSupported);
            AddBody(ApiResponseCodes.UsdtNotSupported);
            
            // Trading codes 30_000 - 30_099
            AddBody(ApiResponseCodes.TradingPositionInvalidOrder);
            AddBody(ApiResponseCodes.TradingPositionInsufficientMargin);
            AddBody(ApiResponseCodes.TradingLowBalance);
            AddBody(ApiResponseCodes.TradingAgentAlreadyExists);
            AddBody(ApiResponseCodes.TradingUnknownError);
            
            AddBody(ApiResponseCodes.OperationNotSupported);
            AddBody(ApiResponseCodes.LowBalanceForTransaction);
            AddBody(ApiResponseCodes.LowDestinationBalance);
            AddBody(ApiResponseCodes.InvalidPeerType);
            AddBody(ApiResponseCodes.TransactionNotFound);
            AddBody(ApiResponseCodes.OperationNotAvailable);
            AddBody(ApiResponseCodes.DestinationWalletNotFound);
            AddBody(ApiResponseCodes.DestinationAssetNotFound);
            AddBody(ApiResponseCodes.DestinationAddressIsSuspended);
            AddBody(ApiResponseCodes.DestinationAddressIsNotValid);
            AddBody(ApiResponseCodes.MissingTag);
            AddBody(ApiResponseCodes.InvalidSourceType);
            AddBody(ApiResponseCodes.InvalidDestination);
            AddBody(ApiResponseCodes.InvalidAmount);
            AddBody(ApiResponseCodes.InvalidTag);
            AddBody(ApiResponseCodes.UnsupportedBlockchain);
            AddBody(ApiResponseCodes.FeeTooLow);
            AddBody(ApiResponseCodes.InvalidAsset);
            AddBody(ApiResponseCodes.UnableToEstimateFee);
            AddBody(ApiResponseCodes.AddressIsNotUnique);
            AddBody(ApiResponseCodes.AddressDuplicate);
            AddBody(ApiResponseCodes.AddressNotFound);
            AddBody(ApiResponseCodes.PresetsNotFound);
            
            AddBody(ApiResponseCodes.NotEnoughMargin);
        }

        static void AddBody(ApiResponseCodes code, params string[] keys) => TemplateBodyParams[code] =
            keys?.Select(t => $"${{{t}}}").ToList() ?? new List<string>();

        public static readonly IDictionary<ApiResponseCodes, List<string>> TemplateBodyParams =
            new Dictionary<ApiResponseCodes, List<string>>();
    }
}