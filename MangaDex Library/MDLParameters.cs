using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace MangaDex_Library
{
    public static class MDLParameters
    {
        public static string MangaID = null, Language = "en";
        public static bool DataSaving = false;

        internal static HttpClient client = new HttpClient();
        private static bool setHeaders = false;
        internal static bool AgentSet = false;
        internal readonly static Regex titleSanitationRegex = new Regex("[^a-zA-Z0-9 ]");

        public static void SetUserAgent(string userAgent)
        {
            if (setHeaders == false)
            {
                setHeaders = true;
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            }
            client.DefaultRequestHeaders.UserAgent.Clear();
            client.DefaultRequestHeaders.UserAgent.ParseAdd(userAgent);
            AgentSet = true;
        }
    }

    public class ToStringJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }

        public override bool CanRead
        {
            get { return false; }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }


    [JsonConverter(typeof(ToStringJsonConverter))]
    public class MDNumber
    {
        private List<decimal> places = new List<decimal>();
        private List<string> separators = new List<string>();
        private int Count => places.Count;
        
        private readonly Regex numberGroups = new Regex("^-?[0-9]+|[0-9]+");
        private readonly Regex separatorGroups = new Regex("(?!^-)[^0-9]+");

        public MDNumber(string input)
        {
            foreach (Match match in separatorGroups.Matches(input))
                separators.Add(match.Value);

            foreach (Match match in numberGroups.Matches(input))
            {
                decimal temp = Convert.ToDecimal(match.Value);
                if (temp.ToString("G", CultureInfo.InvariantCulture) != match.Value) // Looking for nonsense like 1.003.2
                    temp = Convert.ToDecimal("0." + match.Value); // This should preserve the numbers as-is for comparison
                
                places.Add(temp);
            }
            if (separators.Count < places.Count)
                separators.Add(".");
            else
                separators[separators.Count - 1] = ".";

            if (places.Count > 1) // Trimming trailing zeroes
            {
                int lastNonZero = 0;
                for (int i = 1; i < places.Count; i++)
                    if (places[i] != 0M)
                        lastNonZero = i;
                
                if (lastNonZero == places.Count - 1)
                    return;
                List<decimal> temp = new List<decimal>();
                List<string> tempSeparators = new List<string>();
                for (int i = 0; i <= lastNonZero; i++)
                {
                    temp.Add(places[i]);
                    tempSeparators.Add(separators[i]);
                }
                
                places = temp.ToList();
                separators = tempSeparators.ToList();
                separators[separators.Count - 1] = ".";
            }
        }

        public static implicit operator MDNumber(string input)
        {
            return new MDNumber(input);
        }

        public MDNumber(decimal input)
        {
            foreach (Match match in separatorGroups.Matches(input.ToString("G", CultureInfo.InvariantCulture)))
                separators.Add(match.Value);
            
            foreach (Match match in numberGroups.Matches(input.ToString("G", CultureInfo.InvariantCulture)))
            {
                decimal temp = Convert.ToDecimal(match.Value);
                if (temp.ToString("G", CultureInfo.InvariantCulture) != match.Value) // Looking for nonsense like 1.003.2
                    temp = Convert.ToDecimal("0." + match.Value); // This should preserve the numbers as-is for comparison
                
                places.Add(temp);
            }
            if (separators.Count < places.Count)
                separators.Add(".");
            else
                separators[separators.Count - 1] = ".";

            if (places.Count > 1) // Trimming trailing zeroes
            {
                int lastNonZero = 0;
                for (int i = 1; i < places.Count; i++)
                    if (places[i] != 0M)
                        lastNonZero = i;
                
                if (lastNonZero == places.Count - 1)
                    return;
                List<decimal> temp = new List<decimal>();
                List<string> tempSeparators = new List<string>();
                for (int i = 0; i <= lastNonZero; i++)
                {
                    temp.Add(places[i]);
                    tempSeparators.Add(separators[i]);
                }
                
                places = temp.ToList();
                separators = tempSeparators.ToList();
                separators[separators.Count - 1] = ".";
            }
        }

        public static implicit operator MDNumber(decimal input)
        {
            return new MDNumber(input);
        }

        public static implicit operator MDNumber(double input)
        {
            return new MDNumber(Convert.ToDecimal(input));
        }

        public static implicit operator MDNumber(int input)
        {
            return new MDNumber(Convert.ToDecimal(input));
        }

        public MDNumber(MDNumber input)
        {
            places = input.places.ToList();
            separators = input.separators.ToList();
        }

        public override string ToString()
        {
            string output = "";
            for (int i = 0; i < places.Count; i++)
            {
                if (places[i] > 0 && places[i] < 1)
                    output += places[i].ToString("G", CultureInfo.InvariantCulture).Substring(2);
                else
                    output += places[i].ToString("G", CultureInfo.InvariantCulture);
                
                output += separators[i];
            }

            return output.Substring(0, output.Length - 1);
        }

        public static implicit operator string(MDNumber input)
        {
            return input.ToString();
        }

        public static bool operator ==(MDNumber nr1, object nr2)
        {
            if (nr1 is null && nr2 is null)
                return true;
            else if ((!(nr1 is null) || !(nr2 is null)) && (nr1 is null || nr2 is null))
                return false;
            else if (nr2 is MDNumber nr)
            {
                if (nr1.Count != nr.Count)
                    return false;

                bool foundDiff = false;
                for (int i = 0; i < nr1.Count; i++)
                {
                    if (nr1.places[i] != nr.places[i])
                    {
                        foundDiff = true;
                        break;
                    }
                }

                return !foundDiff;
            }
            throw new Exception($"Cannot compare MDNumber with {nr2.GetType()}!");
        }

        public static bool operator !=(MDNumber nr1, object nr2)
        {
            if (nr1 is null && nr2 is null)
                return false;
            else if ((!(nr1 is null) || !(nr2 is null)) && (nr1 is null || nr2 is null))
                return true;
            else if (nr2 is MDNumber nr)
            {
                if (nr1.Count != nr.Count)
                    return true;

                bool foundDiff = false;
                for (int i = 0; i < nr1.Count; i++)
                {
                    if (nr1.places[i] != nr.places[i])
                    {
                        foundDiff = true;
                        break;
                    }
                }

                return foundDiff;
            }
            throw new Exception($"Cannot compare MDNumber with {nr2.GetType()}!");
        }

        public override bool Equals(object obj)
        {
            if (!(obj is MDNumber))
                return false;
            
            return this == (obj as MDNumber);
        }

        public override int GetHashCode()
        {
            return places.GetHashCode();
        }

        public static bool operator <(MDNumber nr1, object nr2)
        {
            if (nr1 is null || nr2 is null)
                return false;

            MDNumber temp = (MDNumber)nr2;

            int count;
            if (nr1.Count < temp.Count)
                count = nr1.Count;
            else
                count = temp.Count;
            
            for (int i = 0; i < count; i++)
                if (nr1.places[i] < temp.places[i])
                    return true;
                else if (nr1.places[i] > temp.places[i])
                    return false;

            if (nr1.Count < temp.Count)
                return true;

            return false;
        }

        public static bool operator <=(MDNumber nr1, object nr2)
        {
            if (nr1 == nr2 || nr1 < nr2)
                return true;
            else
                return false;
        }

        public static bool operator >(MDNumber nr1, object nr2)
        {
            if (nr1 is null || nr2 is null)
                return false;

            MDNumber temp = (MDNumber)nr2;
            
            int count;
            if (nr1.Count < temp.Count)
                count = nr1.Count;
            else
                count = temp.Count;
            
            for (int i = 0; i < count; i++)
                if (nr1.places[i] > temp.places[i])
                    return true;
                else if (nr1.places[i] < temp.places[i])
                    return false;

            if (nr1.Count > temp.Count)
                return true;
            
            return false;
        }

        public static bool operator >=(MDNumber nr1, object nr2)
        {
            if (nr1 == nr2 || nr1 > nr2)
                return true;
            else
                return false;
        }

        #region Operator cases
        public static bool operator ==(MDNumber nr1, int nr2)
        {
            return nr1 == new MDNumber(Convert.ToDecimal(nr2));
        }

        public static bool operator !=(MDNumber nr1, int nr2)
        {
            return nr1 != new MDNumber(Convert.ToDecimal(nr2));
        }

        public static bool operator ==(MDNumber nr1, decimal nr2)
        {
            return nr1 == new MDNumber(nr2);
        }

        public static bool operator !=(MDNumber nr1, decimal nr2)
        {
            return nr1 != new MDNumber(nr2);
        }

        public static bool operator ==(int nr2, MDNumber nr1)
        {
            return nr1 == new MDNumber(Convert.ToDecimal(nr2));
        }

        public static bool operator !=(int nr2, MDNumber nr1)
        {
            return nr1 != new MDNumber(Convert.ToDecimal(nr2));
        }

        public static bool operator ==(decimal nr2, MDNumber nr1)
        {
            return nr1 == new MDNumber(nr2);
        }

        public static bool operator !=(decimal nr2, MDNumber nr1)
        {
            return nr1 != new MDNumber(nr2);
        }

        public static bool operator <(MDNumber nr1, int nr2)
        {
            return nr1 < new MDNumber(Convert.ToDecimal(nr2));
        }

        public static bool operator <=(MDNumber nr1, int nr2)
        {
            return nr1 <= new MDNumber(Convert.ToDecimal(nr2));
        }

        public static bool operator >(MDNumber nr1, int nr2)
        {
            return nr1 > new MDNumber(Convert.ToDecimal(nr2));
        }

        public static bool operator >=(MDNumber nr1, int nr2)
        {
            return nr1 >= new MDNumber(Convert.ToDecimal(nr2));
        }

        public static bool operator <(int nr2, MDNumber nr1)
        {
            return nr1 < new MDNumber(Convert.ToDecimal(nr2));
        }

        public static bool operator <=(int nr2, MDNumber nr1)
        {
            return nr1 <= new MDNumber(Convert.ToDecimal(nr2));
        }

        public static bool operator >(int nr2, MDNumber nr1)
        {
            return nr1 > new MDNumber(Convert.ToDecimal(nr2));
        }

        public static bool operator >=(int nr2, MDNumber nr1)
        {
            return nr1 >= new MDNumber(Convert.ToDecimal(nr2));
        }

        public static bool operator <(MDNumber nr1, decimal nr2)
        {
            return nr1 < new MDNumber(nr2);
        }

        public static bool operator <=(MDNumber nr1, decimal nr2)
        {
            return nr1 <= new MDNumber(nr2);
        }

        public static bool operator >(MDNumber nr1, decimal nr2)
        {
            return nr1 > new MDNumber(nr2);
        }

        public static bool operator >=(MDNumber nr1, decimal nr2)
        {
            return nr1 >= new MDNumber(nr2);
        }

        public static bool operator <(decimal nr2, MDNumber nr1)
        {
            return nr1 < new MDNumber(nr2);
        }

        public static bool operator <=(decimal nr2, MDNumber nr1)
        {
            return nr1 <= new MDNumber(nr2);
        }

        public static bool operator >(decimal nr2, MDNumber nr1)
        {
            return nr1 > new MDNumber(nr2);
        }

        public static bool operator >=(decimal nr2, MDNumber nr1)
        {
            return nr1 >= new MDNumber(nr2);
        }
        #endregion

        public void operator ++()
        {
            decimal firstPlace = places[0];
            places.Clear();
            places.Add(firstPlace + 1M);
        }
    
        public static MDNumber operator +(MDNumber nr1, int input)
        {
            return new MDNumber(nr1.places[0] + input);
        }
    }
}
