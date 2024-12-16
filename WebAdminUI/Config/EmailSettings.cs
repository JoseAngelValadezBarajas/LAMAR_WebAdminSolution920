// --------------------------------------------------------------------
// <copyright file="EmailSettings.cs" company="Ellucian">
//     Copyright 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using System.Configuration;

namespace WebAdminUI.Config
{
    /// <summary>
    /// Settings
    /// </summary>
    /// <seealso cref="System.Configuration.ConfigurationElement" />
    public class EmailSetting : ConfigurationElement
    {
        /// <summary>
        /// Gets or sets the From.
        /// </summary>
        [ConfigurationProperty("EmailBody")]
        public string EmailBody
        {
            get => (string)this[nameof(EmailBody)];
            set => this[nameof(EmailBody)] = value;
        }

        /// <summary>
        /// Gets or sets the Enable Ssl.
        /// </summary>
        [ConfigurationProperty("EmailSubject")]
        public string EmailSubject
        {
            get => (string)this[nameof(EmailSubject)];
            set => this[nameof(EmailSubject)] = value;
        }
    }

    /// <summary>
    /// MailSettings
    /// </summary>
    public class EmailSettings : ConfigurationSection
    {
        /// <summary>
        /// Gets or sets the ElectronicCerfiticateSetting.
        /// </summary>
        [ConfigurationProperty("ElectronicCerfiticateSetting")]
        public EmailSetting ElectronicCerfiticateSetting
        {
            get => (EmailSetting)this[nameof(ElectronicCerfiticateSetting)];
            set => this[nameof(ElectronicCerfiticateSetting)] = value;
        }

        /// <summary>
        /// Gets or sets the ElectronicDegreeSetting.
        /// </summary>
        [ConfigurationProperty("ElectronicDegreeSetting")]
        public EmailSetting ElectronicDegreeSetting
        {
            get => (EmailSetting)this[nameof(ElectronicDegreeSetting)];
            set => this[nameof(ElectronicDegreeSetting)] = value;
        }

        /// <summary>
        /// Gets or sets the FiscalRecordsSetting.
        /// </summary>
        [ConfigurationProperty("FiscalRecordsSetting")]
        public EmailSetting FiscalRecordsSetting
        {
            get => (EmailSetting)this[nameof(FiscalRecordsSetting)];
            set => this[nameof(FiscalRecordsSetting)] = value;
        }
    }
}