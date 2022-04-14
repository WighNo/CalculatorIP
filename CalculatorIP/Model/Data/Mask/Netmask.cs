using System;
using System.Text;

namespace CalculatorIP.Model.Data.Mask
{
    public class Netmask
    {
        private byte[] _adress;

        private int _numberOfSubnets;
        private int _numberOfAdresses;

        private NetmaskClass _class;

        public Netmask(byte[] adress, int numberOfSubnets, int numberOfAdresses, NetmaskClass netmaskClass)
        {
            _adress = adress;

            _numberOfSubnets = numberOfSubnets;
            _numberOfAdresses = numberOfAdresses;

            _class = netmaskClass;
        }

        public byte[] GetAdress() => _adress;

        public string GetDecimalNotation(char characterBetweenOctets)
        {
            StringBuilder stringBuilder = new StringBuilder();

            for (int i = 0; i < _adress.Length; i++)
            {
                stringBuilder.Append(_adress[i]);
                stringBuilder.Append(characterBetweenOctets);
            }

            return stringBuilder.ToString();
        }

        public int GetSubnetsCount() => _numberOfSubnets;
        public int GetAdressesCount() => _numberOfAdresses;
    }

    public enum NetmaskClass
    {
        A128,
        A64,
        A32,
        A16,
        A8,
        A4,
        A2,
        B1,
        B128,
        B64,
        B32,
        B16,
        B8,
        B4,
        B2,
        C1,
        C1s2,
        C1s4,
        C1s8,
        C1s16,
        C1s32,
        C1s64,
        C1s128,
    }
}
