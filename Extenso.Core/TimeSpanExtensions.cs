using System;
using System.Text;

namespace Extenso
{
    public static class TimeSpanExtensions
    {
        public static string ToFormattedEnglish(this TimeSpan timeSpan, bool useShorthand)
        {
            if (timeSpan == TimeSpan.MaxValue || timeSpan == TimeSpan.MinValue)
            {
                return "Unknown";
            }

            var sb = new StringBuilder();

            if (!useShorthand)
            {
                #region Years & Days

                if (timeSpan.Days == 365 || timeSpan.Days == 366)
                {
                    sb.Append("1 year, ");
                }
                else if (timeSpan.Days > 365)
                {
                    sb.Append((timeSpan.Days / 365) + " years, ");
                }
                else if (timeSpan.Days == 1)
                {
                    sb.Append("1 day, ");
                }
                else if (timeSpan.Days > 1)
                {
                    sb.Append(timeSpan.Days + " days, ");
                }

                #endregion Years & Days

                #region Hours

                if (timeSpan.Hours == 1)
                {
                    sb.Append("1 hour, ");
                }
                else if (timeSpan.Hours > 1)
                {
                    sb.Append(timeSpan.Hours + " hours, ");
                }

                #endregion Hours

                #region Minutes

                if (timeSpan.Minutes == 1)
                {
                    sb.Append("1 minute, ");
                }
                else if (timeSpan.Minutes > 1)
                {
                    sb.Append(timeSpan.Minutes + " minutes, ");
                }

                #endregion Minutes

                #region Seconds

                if (timeSpan.Seconds == 1)
                {
                    sb.Append("1 second, ");
                }
                else if (timeSpan.Seconds > 1)
                {
                    sb.Append(timeSpan.Seconds + " seconds, ");
                }

                #endregion Seconds
            }
            else
            {
                #region Years & Days

                if (timeSpan.Days == 365 || timeSpan.Days == 366)
                {
                    sb.Append("1yr, ");
                }
                else if (timeSpan.Days > 365)
                {
                    sb.Append((timeSpan.Days / 365) + "yr, ");
                }
                else if (timeSpan.Days == 1)
                {
                    sb.Append("1d, ");
                }
                else if (timeSpan.Days > 1)
                {
                    sb.Append(timeSpan.Days + "d, ");
                }

                #endregion Years & Days

                #region Hours

                if (timeSpan.Hours == 1)
                {
                    sb.Append("1h, ");
                }
                else if (timeSpan.Hours > 1)
                {
                    sb.Append(timeSpan.Hours + "h, ");
                }

                #endregion Hours

                #region Minutes

                if (timeSpan.Minutes == 1)
                {
                    sb.Append("1min, ");
                }
                else if (timeSpan.Minutes > 1)
                {
                    sb.Append(timeSpan.Minutes + "min, ");
                }

                #endregion Minutes

                #region Seconds

                if (timeSpan.Seconds == 1)
                {
                    sb.Append("1s, ");
                }
                else if (timeSpan.Seconds > 1)
                {
                    sb.Append(timeSpan.Seconds + "s, ");
                }

                #endregion Seconds
            }

            if (sb.Length >= 2)
            {
                sb.Remove(sb.Length - 2, 2);
            }

            return sb.ToString();
        }
    }
}