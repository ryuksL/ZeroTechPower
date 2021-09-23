using UnityEngine;
using Verse;

namespace LingGame
{
    [StaticConstructorOnStartup]
    public static class LingSSWAUI
    {
        public static Texture2D BreakLink = ContentFinder<Texture2D>.Get("LingUI/BreakLink");

        public static Texture2D PowerDown = ContentFinder<Texture2D>.Get("LingUI/PowerDown");

        public static Texture2D PowerUp = ContentFinder<Texture2D>.Get("LingUI/PowerUp");

        public static Texture2D FindNeeder = ContentFinder<Texture2D>.Get("LingUI/FindNeeder");

        public static Texture2D SelectNeeder = ContentFinder<Texture2D>.Get("LingUI/SelectNeeder");
    }
}