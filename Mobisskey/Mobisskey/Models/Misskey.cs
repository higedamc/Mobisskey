using System;
using System.Collections.Generic;
using System.IO;
using Disboard.Misskey;
using Disboard.Models;
using Newtonsoft.Json;

namespace Mobisskey.Models
{
    public class Misskey : Singleton<Misskey>
    {
        public MisskeyClient Client { get; protected set; }

        public List<Credential> Credentials { get; protected set; }

        static readonly string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "credentials.json");

        public Misskey()
        {
            if (File.Exists(path))
            {
                Credentials = JsonConvert.DeserializeObject<List<Credential>>(File.ReadAllText(path));
            }
            else
            {
                Credentials = new List<Credential>();
            }
        }

        public MisskeyClient SwitchClient(string domain)
        {
            return Client = new MisskeyClient(domain);
        }

        public MisskeyClient SwitchClient(Credential credential)
        {
            return Client = new MisskeyClient(credential);
        }

        public void Save(Credential credential)
        {
            if (Credentials.Contains(credential))
                return;
            Credentials.Add(credential);
            File.WriteAllText(path, JsonConvert.SerializeObject(Credentials));
        }
    }
}
