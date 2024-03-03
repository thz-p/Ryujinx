namespace Ryujinx.UI.Common
{
    /// <summary>
    /// 表示模拟器可能向用户报告的常见错误。
    /// </summary>
    public enum UserError
    {
        /// <summary>
        /// 没有错误需要报告。
        /// </summary>
        Success = 0x0,

        /// <summary>
        /// 没有密钥。
        /// </summary>
        NoKeys = 0x1,

        /// <summary>
        /// 没有固件。
        /// </summary>
        NoFirmware = 0x2,

        /// <summary>
        /// 固件解析失败。
        /// </summary>
        /// <remarks>可能与密钥相关。</remarks>
        FirmwareParsingFailed = 0x3,

        /// <summary>
        /// 给定路径中找不到应用程序。
        /// </summary>
        ApplicationNotFound = 0x4,

        /// <summary>
        /// 未知错误。
        /// </summary>
        Unknown = 0xDEAD,
    }
}