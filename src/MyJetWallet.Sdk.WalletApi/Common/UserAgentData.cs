namespace MyJetWallet.Sdk.WalletApi.Common
{

    public class UserAgentData
    {
        public string ApplicationVersion { get; set; }
        public string Build { get; set; }
        public string Platform { get; set; }
        public string ScreenSize { get; set; }
        public string PixelRatio { get; set; }
        public string Lang { get; set; }
        public string DeviceModel { get; set; }
        public string DeviceUid { get; set; }
        public string InstallationId { get; set; }

    }
}