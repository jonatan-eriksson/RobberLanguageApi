namespace RobberLanguageApi.Models
{
    public class Translation
    {
        public string OriginalSentence { get; set; }
        public string TranslatedSentence { get; set; }

        public Translation() { }

        public Translation(string originalSentence)
        {
            this.OriginalSentence = originalSentence;
            this.TranslatedSentence = TranslateSentence(originalSentence);
        }

        public static string TranslateSentence(string text)
        {
            string translation = "";
            foreach (var letter in text)
            {
                translation += letter;
                if (IsConsonant(letter))
                    translation += $"o{char.ToLower(letter)}";
            }

            return translation;
        }
        private static bool IsConsonant(char c)
        {
            return "BCDFGHJKLMNPQRSTVWXZ".IndexOf(char.ToUpper(c)) >= 0;
        }
    }
}