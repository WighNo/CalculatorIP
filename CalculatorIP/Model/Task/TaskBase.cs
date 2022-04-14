using CalculatorIP.Model.Data.IP;
using System.Linq;
using System.Text;
using static CalculatorIP.Session;

namespace CalculatorIP
{
    public abstract class TaskBase
    {
        protected IPAdressType[] ipAdressTypes = new IPAdressType[3]
        {
            IPAdressType.A,
            IPAdressType.B,
            IPAdressType.C
        };
        protected abstract string[] _content { get; }

        public abstract GeneratedTask GetTask();
        protected abstract string GenerateSolution();
        protected abstract string GetSolutionDescription();

        protected int GetFirstOctet(byte[] byteMask)
        {
            int numberFirstOctet = 1;

            for (int i = 0; i < byteMask.Length; i++)
            {
                int octet = byteMask[i];

                if (octet == 0)
                    numberFirstOctet++;
            }

            return numberFirstOctet;
        }

        protected string MaskWithAllottedBits(string defaultMask, int bitsUsedToDefineTheNetwork, IPAdressType ipAdressType)
        {
            StringBuilder stringBuilder = new StringBuilder();

            string[] maskList = defaultMask.Split('.');

            for(int i = ipAdressType.GetHashCode() + 1; i < maskList.Length; i++)
            {
                stringBuilder.Clear();

                for(int j = 0; j < 8; j++)
                {
                    if(bitsUsedToDefineTheNetwork > 0)
                    {
                        stringBuilder.Append('1');
                        bitsUsedToDefineTheNetwork--;
                    }
                    else
                    {
                        stringBuilder.Append('0');
                    }
                }

                maskList[i] = stringBuilder.ToString();
            }

            stringBuilder.Clear();

            for (int i = 0; i < maskList.Length; i++)
            {
                stringBuilder.Append(maskList[i]);

                if (i < maskList.Length - 1)
                    stringBuilder.Append('.');
            }

            return stringBuilder.ToString();
        }

        protected string MaskWithAllottedBits(string mask, int freeBitsCount)
        {
            StringBuilder stringBuilder = new StringBuilder();

            string[] maskList = mask.Split('.');

            for(int i = maskList.Length - 1; i > 0; i--)
            {
                stringBuilder.Clear();

                for(int j = 0; j < 8; j++)
                {
                    if(freeBitsCount > 0)
                    {
                        stringBuilder.Append('0');
                        freeBitsCount--;
                    }
                    else
                    {
                        stringBuilder.Append('1');
                    }
                }

                maskList[i] = new  string(stringBuilder.ToString().Reverse().ToArray());
            }

            stringBuilder.Clear();

            for (int i = 0; i < maskList.Length; i++)
            {
                stringBuilder.Append(maskList[i]);

                if (i < maskList.Length - 1)
                    stringBuilder.Append('.');
            }

            return stringBuilder.ToString();
        }

        protected void TheNumberToClosestPowerOfTwo()
        {

        }
    }
}
