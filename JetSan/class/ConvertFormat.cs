using System;

namespace HyTemplate
{
    public class ConvertFormat
    {
        public static string GetITR(int m_data)
        {
            string data = Math.Pow(10, ((m_data * 10 / 4000) - 10.625)).ToString("0.00E+0");  //INFICON IKR 251 高真空計  Torr = 10.625, Pa=8.5, mbar=10.5
            return data;
        }
        public static string GetTTR(int m_data)
        {
            double buf = Math.Pow(10, m_data * 10 / 4000 - 5.625);
            string data = (buf > 750) ? 750.ToString("0.00E+0") : buf.ToString("0.00E+0");
            return data;
        }
    }
}
