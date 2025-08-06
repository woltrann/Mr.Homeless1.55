// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("wl8zE88lO56bdC6uiUldDTid16y+PTM8DL49Nj6+PT08psEwNEgi8tp2jydhkWfbpPuWRP0y7xpmCE8L+2bTwY1d2WJ/PMYCqlOd7v/ak+QMvj0eDDE6NRa6dLrLMT09PTk8P7KKkmF+09aA0vb7uuCmqXNQE734d8hYt5vZQ9TjqLnwbeCIg+y06AlWzQevP2m9zbnC0xne9t1/7YlDsjZ1lhIRCZaQ0Mo8ZXKEfmK8WeYjB0uyFVm+yDXDVo16ckWDcboNBOkgGgDWI55ez875cl+p1V846SXNYNENjBt/A26UM29qeXjUt/KMRve9tp2drFzvgABEWG6T9Id2qD/pYY2K9KTGpbk1Sk8TMLUu/nS5hE5dthnANzx3xEapVT4/PTw9");
        private static int[] order = new int[] { 2,1,6,7,6,5,8,11,11,10,10,12,13,13,14 };
        private static int key = 60;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
