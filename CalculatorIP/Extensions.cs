
using System.Text;

namespace System
{
    public static class Extensions
    {
        public static string GetBinaryData(this byte[] adress, char characterBetweenOctets)
        {
            StringBuilder stringBuilder = new StringBuilder();

            for (int i = 0; i < adress.Length; i++)
            {
                stringBuilder.Append(Convert.ToString(adress[i], 2).PadLeft(8, '0'));

                if (i < adress.Length - 1)
                    stringBuilder.Append(characterBetweenOctets);
            }

            return stringBuilder.ToString();
        }

        public static int ToIntInBinaryString(this string binaryOctet)
        {
            return Convert.ToInt32(binaryOctet, 2);
        }
    }
}