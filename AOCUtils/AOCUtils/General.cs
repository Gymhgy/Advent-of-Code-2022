using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Net;

namespace AOCUtils {
    public static class General {

        public static List<int> Ints(this string s) {
            return Regex.Matches(s, @"-?\d+").Select(x => int.Parse(x.Value)).ToList();
        }

        public static async Task GetInput(int day, string cookie, string filename, int year = 2022) {
            if (!File.Exists(filename)) {
                var uri = new Uri("https://adventofcode.com");
                var cookies = new CookieContainer();
                cookies.Add(uri, new System.Net.Cookie("session", cookie));
                using var file = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None);
                using var handler = new HttpClientHandler() { CookieContainer = cookies };
                using var client = new HttpClient(handler) { BaseAddress = uri };
                using var response = await client.GetAsync($"/{year}/day/{day}/input");
                using var stream = await response.Content.ReadAsStreamAsync();
                await stream.CopyToAsync(file);
            }
        }

        public static void Lines(this string s, Action<string> f) {
            foreach(var l in s.Split("\n")) {
                f(l);
            }
        }
        public static void Lines(this string s, Action<string, int> f) {
            int i = 0;
            foreach (var l in s.Split("\n")) {
                f(l, i++);
            }
        }
        public static List<string> Lines(this string s) => s.Split("\n").ToList();
        public static void Unchunk(this string s, Action<string> f) {
            foreach (var l in s.Split("\n\n")) {
                f(l);
            }
        }

        public static void F(this object obj) => Console.WriteLine(Microsoft.CodeAnalysis.CSharp.Scripting.Hosting.CSharpObjectFormatter.Instance.FormatObject(obj));

    }
}