using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using RepositoryPattern.Data;
using RepositoryPattern.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RepositoryPattern.Services
{
    public class DBService : IDBService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _http;
        public static IConfigurationRoot Configuration;

        public DBService(ApplicationDbContext context, IHttpContextAccessor http)
        {
            _context = context;
            _http = http;
        }

        private string GetComputerName(string clientIP)
        {
            try
            {
                var hostEntry = Dns.GetHostEntry(clientIP);
                return hostEntry.HostName;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        private static string GetExternalIP()
        {
            try
            {
                string externalIP;
                externalIP = (new WebClient()).DownloadString("http://checkip.dyndns.org/");
                externalIP = (new Regex(@"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}"))
                    .Matches(externalIP)[0].ToString();
                return externalIP;
            }
            catch { return null; }
        }

        public string GeneratePassword(int maxSize)
        {
            var passwords = string.Empty;
            var chArray1 = new char[52];
            var chArray2 = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^*()_+".ToCharArray();
            var data1 = new byte[1];
            using (var cryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                cryptoServiceProvider.GetNonZeroBytes(data1);
                var data2 = new byte[maxSize];
                cryptoServiceProvider.GetNonZeroBytes(data2);
                var stringBuilder = new StringBuilder(maxSize);
                foreach (var num in data2)
                    stringBuilder.Append(chArray2[(int)num % chArray2.Length]);
                passwords = stringBuilder.ToString();
                var number = Random("N");
                var upper = Random("S");
                var lower = Random("l");
                passwords += number + upper + lower;
                return passwords;
            }
        }

        public string Random(string type)
        {
            var data2 = new byte[1];
            var passwords = string.Empty;
            switch (type)
            {
                case "N":
                    {
                        var charArray = "0123456789";
                        var stringBuilder = new StringBuilder(2);
                        foreach (var num in data2)
                            stringBuilder.Append(charArray[(int)num % charArray.Length]);
                        passwords = stringBuilder.ToString();
                        return passwords;
                    }

                case "l":
                    {
                        var charArray = "abcdefghijklmnopqrstuvwxyz";

                        var stringBuilder = new StringBuilder(2);
                        foreach (var num in data2)
                            stringBuilder.Append(charArray[(int)num % charArray.Length]);
                        passwords = stringBuilder.ToString();
                        return passwords;
                    }

                case "C":
                    {
                        var charArray = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

                        var stringBuilder = new StringBuilder(2);
                        foreach (var num in data2)
                            stringBuilder.Append(charArray[(int)num % charArray.Length]);
                        passwords = stringBuilder.ToString();
                        return passwords;
                    }

                case "S":
                    {
                        var charArray = "!@#$%^&*()_+-={}|[]:;<>?,./";
                        var stringBuilder = new StringBuilder(2);
                        foreach (var num in data2)
                            stringBuilder.Append(charArray[(int)num % charArray.Length]);
                        passwords = stringBuilder.ToString();
                        return passwords;
                    }
            }

            return string.Empty;
        }

        public static string DBConnection()
        {
            try
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .AddEnvironmentVariables();
                builder.AddEnvironmentVariables();
                Configuration = builder.Build();
                string connectionstring = Configuration.GetConnectionString("DefaultConnection");
                return connectionstring;
            }
            catch
            {
                return "";
            }
        }
    }
}