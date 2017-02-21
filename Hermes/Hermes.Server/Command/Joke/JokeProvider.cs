using Hermes.Model;
using System;

namespace Hermes.Server.Command.Joke
{
    // TODO: Doc.
    internal sealed class JokeProvider
    {
        private static int JokeNumber
        {
            get
            {
                return JokeProvider.Jokes.Length;
            }
        }
        private static TocTocJoke[] Jokes;

        static JokeProvider()
        {
            Jokes = new TocTocJoke[2];
            Jokes[0] = new TocTocJoke()
            {
                ServerInteractions = new string[]
                {
                    TocTocJoke.EntryText,
                    "Você sabe",
                    "AVADA KEDAVRA!"
                },
                ClientInteractions = new string[]
                {
                    TocTocJoke.ForcedInteraction,
                    "Você sabe quem?"
                }
            };
            Jokes[1] = new TocTocJoke()
            {
                ServerInteractions = new string[]
                {
                    TocTocJoke.EntryText,
                    "Bará",
                    "Baráquem obama"
                },
                ClientInteractions = new string[]
                {
                    TocTocJoke.ForcedInteraction,
                    "Bará quem?"
                }
            };
        }

        public static TocTocJoke GetNew()
        {
            Random random = new Random();
            return JokeProvider.Jokes[random.Next(JokeProvider.JokeNumber)]
                .Clone() as TocTocJoke;
        }
    }
}
