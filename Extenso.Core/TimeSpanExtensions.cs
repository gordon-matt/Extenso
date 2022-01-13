// 2018.06.24 - Will probably remove this. Commenting out for now, just in case...

//using System;
//using System.Text;

//namespace Extenso
//{
//    /// <summary>
//    /// Provides a set of static methods for querying and manipulating instances of System.TimeSpan
//    /// </summary>
//    public static class TimeSpanExtensions
//    {
//        /// <summary>
//        ///
//        /// </summary>
//        /// <param name="source"></param>
//        /// <param name="useShorthand"></param>
//        /// <returns></returns>
//        public static string ToFormattedEnglish(this TimeSpan source, bool useShorthand)
//        {
//            if (source == TimeSpan.MaxValue || source == TimeSpan.MinValue)
//            {
//                return "Unknown";
//            }

//            var sb = new StringBuilder();

//            if (!useShorthand)
//            {
//                #region Years & Days

//                if (source.Days == 365 || source.Days == 366)
//                {
//                    sb.Append("1 year, ");
//                }
//                else if (source.Days > 365)
//                {
//                    sb.Append((source.Days / 365) + " years, ");
//                }
//                else if (source.Days == 1)
//                {
//                    sb.Append("1 day, ");
//                }
//                else if (source.Days > 1)
//                {
//                    sb.Append(source.Days + " days, ");
//                }

//                #endregion Years & Days

//                #region Hours

//                if (source.Hours == 1)
//                {
//                    sb.Append("1 hour, ");
//                }
//                else if (source.Hours > 1)
//                {
//                    sb.Append(source.Hours + " hours, ");
//                }

//                #endregion Hours

//                #region Minutes

//                if (source.Minutes == 1)
//                {
//                    sb.Append("1 minute, ");
//                }
//                else if (source.Minutes > 1)
//                {
//                    sb.Append(source.Minutes + " minutes, ");
//                }

//                #endregion Minutes

//                #region Seconds

//                if (source.Seconds == 1)
//                {
//                    sb.Append("1 second, ");
//                }
//                else if (source.Seconds > 1)
//                {
//                    sb.Append(source.Seconds + " seconds, ");
//                }

//                #endregion Seconds
//            }
//            else
//            {
//                #region Years & Days

//                if (source.Days == 365 || source.Days == 366)
//                {
//                    sb.Append("1yr, ");
//                }
//                else if (source.Days > 365)
//                {
//                    sb.Append((source.Days / 365) + "yr, ");
//                }
//                else if (source.Days == 1)
//                {
//                    sb.Append("1d, ");
//                }
//                else if (source.Days > 1)
//                {
//                    sb.Append(source.Days + "d, ");
//                }

//                #endregion Years & Days

//                #region Hours

//                if (source.Hours == 1)
//                {
//                    sb.Append("1h, ");
//                }
//                else if (source.Hours > 1)
//                {
//                    sb.Append(source.Hours + "h, ");
//                }

//                #endregion Hours

//                #region Minutes

//                if (source.Minutes == 1)
//                {
//                    sb.Append("1min, ");
//                }
//                else if (source.Minutes > 1)
//                {
//                    sb.Append(source.Minutes + "min, ");
//                }

//                #endregion Minutes

//                #region Seconds

//                if (source.Seconds == 1)
//                {
//                    sb.Append("1s, ");
//                }
//                else if (source.Seconds > 1)
//                {
//                    sb.Append(source.Seconds + "s, ");
//                }

//                #endregion Seconds
//            }

//            if (sb.Length >= 2)
//            {
//                sb.Remove(sb.Length - 2, 2);
//            }

//            return sb.ToString();
//        }
//    }
//}