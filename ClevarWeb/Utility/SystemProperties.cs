using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using ClevarWeb.Data;
using ClevarWeb.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace ClevarWeb.Utility
{
    // https://refactoring.guru/design-patterns/singleton/csharp/example
    public sealed class SystemProperties
    {
        private static readonly Lazy<SystemProperties> lazy = new Lazy<SystemProperties>(() => new SystemProperties());
        public static SystemProperties Instance { get { return lazy.Value; } }

        private SystemProperties()
        {
            this.Version = GetAppSettingsNodeValue("Version");
            this.LoginTimeoutMinutes = int.Parse(GetAppSettingsNodeValue("LoginTimeoutMinutes"));
            InitialiseAdministrativeUserCredentials();
            this.IsDbConnected = false;
        }

        public string Version { get; set; }
        public string Major { get { return Version.Split('.')[0]; } }
        public string Minor { get { return Version.Split('.')[1]; } }
        public string Revision { get { return Version.Split('.')[2]; } }

        public string Username { get; set; }
        public string Password { get; set; }
        public int LoginTimeoutMinutes { get; set; }
        public bool IsDebugMode { get => Debugger.IsAttached; }

        public SystemSetting SystemSettings { get; set; }

        public DateTime? LastDbCheckDateTime { get; set; } = null;
        public bool IsDbConnected { get; set; }
        public string DbConnectionStatus { get; set; } = "";

        public bool GetIsDbConnected<dbset>(dbset db)where dbset: ClevarDbContext
        {
            if(LastDbCheckDateTime != null)
            {
                var duration = DateTime.Now.Subtract(LastDbCheckDateTime.Value);
                if(duration.Minutes < 5)
                    return IsDbConnected;
            }

            DbConnectionStatus = "";
            LastDbCheckDateTime = DateTime.Now;

            try
            {
                db.SystemSettings.Any();
                IsDbConnected = true;
                DbConnectionStatus = "Database connection OK.";
            }
            catch (Exception ex)
            {
                DbConnectionStatus = "Database connection FAILURE. " + ex.Message;
                General.Instance.WriteToLog(DbConnectionStatus, Microsoft.Extensions.Logging.LogLevel.Critical);
                IsDbConnected = false;
            }

            return IsDbConnected;
        }

        public void IncreaseRevision(IConfiguration configuration)
        {
            if(configuration.GetSection("Version").Exists())
            {
                string[] version = configuration.GetSection("Version").Value.Split('.');
                int revision = int.Parse(version[2]);

                if (Debugger.IsAttached)
                    revision++;

                version[2] = revision.ToString();
                this.Version = string.Join('.', version);
                UpdateAppSettingsNode("Version", this.Version);
            }
            else
            {
                this.Version = "1.0.0";
                UpdateAppSettingsNode("Version", this.Version);
            }
        }

        private void InitialiseAdministrativeUserCredentials()
        {
            if (GetAppSettingsNodeValue("Username").IsEmpty())
                UpdateAppSettingsNode("Username", "admin");

            if (GetAppSettingsNodeValue("Password").IsEmpty())
                UpdateAppSettingsNode("Password", "admin");

            Username = GetAppSettingsNodeValue("Username");
            Password = GetAppSettingsNodeValue("Password");
        }

        private static void UpdateAppSettingsNode(string key, string value)
        {
            try
            {
                string workingDirectory = Environment.CurrentDirectory;
                var appSettingsJsonFilePath = System.IO.Path.Combine(workingDirectory, "appsettings.json");

                var json = System.IO.File.ReadAllText(appSettingsJsonFilePath);
                Newtonsoft.Json.Linq.JObject jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject<Newtonsoft.Json.Linq.JObject>(json);

                if (jsonObj.ContainsKey(key))
                {
                    jsonObj[key] = value;
                }
                else
                {
                    jsonObj.Add(key, value);
                }

                string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);

                System.IO.File.WriteAllText(appSettingsJsonFilePath, output);
            }
            catch (Exception ex)
            {
                var msg = ex.Message + Environment.NewLine + ex.ToString();
                General.Instance.WriteToLog(msg, Microsoft.Extensions.Logging.LogLevel.Critical);
            }
        }

        public static string GetAppSettingsNodeValue(string key)
        {
            string workingDirectory = Environment.CurrentDirectory;
            var appSettingsJsonFilePath = System.IO.Path.Combine(workingDirectory, "appsettings.json");
            var json = System.IO.File.ReadAllText(appSettingsJsonFilePath);
            dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject<Newtonsoft.Json.Linq.JObject>(json);

            if (jsonObj.ContainsKey(key))
            {
                return jsonObj[key];
            }
            else
            {
                return null;
            }
            
        }
    }
}
