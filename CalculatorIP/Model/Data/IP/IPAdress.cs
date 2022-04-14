using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorIP.Model.Data.IP
{
    public class IPAdress
    {
        private IPAdressType _ipAdressType;

        private byte[] _adress = new byte[4];

        public IPAdress(IPAdressType typeIP, byte[] adress)
        {
            _ipAdressType = typeIP;
            _adress = adress;
        }

        public byte[] GetIP() => _adress;

        public string GetIP(char characterBetweenOctets)
        {
            StringBuilder stringBuilder = new StringBuilder();

            for (int i = 0; i < _adress.Length; i++)
            {
                stringBuilder.Append(_adress[i]);

                if (i < _adress.Length - 1)
                    stringBuilder.Append(characterBetweenOctets);
            }

            return stringBuilder.ToString();
        }
    }
}
