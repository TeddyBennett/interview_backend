using System.Globalization;

namespace AspStudio.Common
{
    public class GStr
    {
        public string Right(string str, int Length)
        {
            if (Length > 0 && str.Length >= Length)
            {
                return str.Substring(str.Length - Length, Length);
            }
            else
            {
                return str;
            }
        }

        public string Left(string str, int Length)
        {
            if (Length > 0 && str.Length >= Length)
            {
                return str.Substring(0, Length);
            }
            else
            {
                return str;
            }
        }

        public string Mid(string str, int strStart, int strLength)
        {
            return str.Substring(strStart - 1, strLength);
        }

        public string fncGetDataCode(string pvSCode, string lvSyntax)
        {
            try
            {
                string lvCode = "";
                for (int i = 1; i <= pvSCode.Length; i++)
                {
                    if (Mid(pvSCode, i, 1) != lvSyntax)
                        lvCode = lvCode + Mid(pvSCode, i, 1);
                    else
                        break;
                }

                return lvCode.Trim();
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public double fncToDouble(string strNumber)
        {//ฟังก์ชั่นแปลงตัวเลขใน string ให้เป็นตัวเลข 
            //แปลงค่าว่างได้ ,แปลงค่าที่มี comma ได้ เช่น 123,456.00
            //แปลงมีค่าลบได้ทั้งหน้าและหลัง เช่น 123,456.00-
            //แปลงค่าติดลบที่เป็นวงเล็บได้ เช่น (123,456.00) --> -123456

            try
            {
                return double.Parse(strNumber.Trim(), System.Globalization.NumberStyles.Any);
            }
            catch //(Exception ex)
            {
                //MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return 0;
            }
        }

        public float fncToFloat(string strNumber)
        {//ฟังก์ชั่นแปลงตัวเลขใน string ให้เป็นตัวเลข 
            //แปลงค่าว่างได้ ,แปลงค่าที่มี comma ได้ เช่น 123,456.00
            //แปลงมีค่าลบได้ทั้งหน้าและหลัง เช่น 123,456.00-
            //แปลงค่าติดลบที่เป็นวงเล็บได้ เช่น (123,456.00) --> -123456

            try
            {
                return float.Parse(strNumber.Trim(), System.Globalization.NumberStyles.Any);
            }
            catch //(Exception ex)
            {
                //MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return 0;
            }
        }

        public int fncToInt(string strNumber)
        {//ฟังก์ชั่นแปลงตัวเลขใน string ให้เป็นเลขจำนวนเต็ม  (+-2,147,483,647) 
            //แปลงค่าว่างได้ ,แปลงค่าที่มี comma ได้ เช่น 123,456
            //แปลงมีค่าลบได้ทั้งหน้าและหลัง เช่น 123,456.00-
            //แปลงค่าติดลบที่เป็นวงเล็บได้ เช่น (123,456.00) --> -123456

            try
            {
                int lvPosition = strNumber.IndexOf("."); //ค้นหาตำแหน่งจุดทศนิยม
                if (lvPosition > 0) strNumber = Left(strNumber, lvPosition); //เอาเฉพาะจำนวนเต็ม ตัดจุดทศนิยมออก

                return int.Parse(strNumber.Trim(), System.Globalization.NumberStyles.Any);
            }
            catch //(Exception ex)
            {
                return 0;
            }
        }

        public string fncChangeSDate(string strDate)
        {
            //--เก็บประเภทของปฏิทิน
            var myCal = DateTimeFormatInfo.CurrentInfo.Calendar;

            string lvYear = Left(strDate, 4);

            //--แปลงปีให้อยู่ในรูปแบบของตัวเลข
            int lvYear2 = fncToInt(lvYear);
            int lvYear3 = lvYear2;

            if (lvYear2 < 2500)
            {
                //--ตรวจว่าปฎิทินของเครื่องเป็นแบบปี พ.ศ. หรือไม่ ถ้าใช่ให้เปลี่ยนก่อนแสดงผล
                if (myCal.ToString() == "System.Globalization.ThaiBuddhistCalendar" || lvYear3 < 2500)
                {
                    //lvYear3 = lvYear2 + 543;
                }
            }
            else if (lvYear2 > 3000)
            {
                lvYear2 -= 543;
            }
            else if (lvYear2 > 2500)
            {
                lvYear2 -= 543;
            }


            //ฟังก์ชั่น Show Date รูปแบบ dd/MM/yyyy
            if (strDate.Length == 8)
            {
                string lvDay = Right(strDate, 2);
                string lvMonth = Mid(strDate, 5, 2);

                return lvDay + "/" + lvMonth + "/" + lvYear3;
            }
            else
            {
                return strDate;
            }
        }

        public string fncChangeTDate(string strDate)
        {
            //--เก็บประเภทของปฏิทิน
            var myCal = DateTimeFormatInfo.CurrentInfo.Calendar;

            //ฟังก์ชั่น แปลงวันที่ เป็นรูปแบบบ Text  รูปแบบ yyyyMMdd
            if (strDate.Length == 10)
            {
                string lvDay = Left(strDate, 2);
                string lvMonth = Mid(strDate, 4, 2);
                string lvYear = Right(strDate, 4);

                int lvNumYear = int.Parse(lvYear);

                if (lvNumYear > 2500)
                {
                    ////--ตรวจว่าปฎิทินของเครื่องเป็นแบบปี พ.ศ. หรือไม่ ถ้าใช่ให้เปลี่ยนก่อนแสดงผล
                    //if (myCal.ToString() == "System.Globalization.ThaiBuddhistCalendar")
                    //{
                    lvNumYear = lvNumYear - 543;
                    lvYear = lvNumYear.ToString();
                    //}
                }
                else if (lvNumYear > 3000)
                {
                    lvNumYear -= 543;
                }

                return lvYear + lvMonth + lvDay;
            }
            if (strDate.Length == 9)
            {
                string[] lvArr = strDate.Split('/');

                string lvDay = fncToInt(lvArr[0]).ToString("00");
                string lvMonth = fncToInt(lvArr[1]).ToString("00");
                string lvYear = lvArr[2];

                int lvNumYear = int.Parse(lvYear);

                if (lvNumYear > 2500)
                {
                    ////--ตรวจว่าปฎิทินของเครื่องเป็นแบบปี พ.ศ. หรือไม่ ถ้าใช่ให้เปลี่ยนก่อนแสดงผล
                    //if (myCal.ToString() == "System.Globalization.ThaiBuddhistCalendar")
                    //{
                    lvNumYear = lvNumYear - 543;
                    lvYear = lvNumYear.ToString();
                    //}
                }
                else if (lvNumYear > 3000)
                {
                    lvNumYear -= 543;
                }

                return lvYear + lvMonth + lvDay;
            }
            if (strDate.Length == 8)
            {
                string[] lvArr = strDate.Split('/');

                string lvDay = fncToInt(lvArr[0]).ToString("00");
                string lvMonth = fncToInt(lvArr[1]).ToString("00");
                string lvYear = lvArr[2];

                int lvNumYear = int.Parse(lvYear);

                if (lvNumYear > 2500)
                {
                    ////--ตรวจว่าปฎิทินของเครื่องเป็นแบบปี พ.ศ. หรือไม่ ถ้าใช่ให้เปลี่ยนก่อนแสดงผล
                    //if (myCal.ToString() == "System.Globalization.ThaiBuddhistCalendar")
                    //{
                    lvNumYear = lvNumYear - 543;
                    lvYear = lvNumYear.ToString();
                    //}
                }
                else if (lvNumYear > 3000)
                {
                    lvNumYear -= 543;
                }

                return lvYear + lvMonth + lvDay;
            }
            else
            {
                return strDate;
            }
        }

        public string ConvertToThaiBaht(string txt)
        {
            string bahtTxt, n, bahtTH = "";
            double amount;
            try { amount = Convert.ToDouble(txt); }
            catch { amount = 0; }
            bahtTxt = amount.ToString("####.00");
            string[] num = { "ศูนย์", "หนึ่ง", "สอง", "สาม", "สี่", "ห้า", "หก", "เจ็ด", "แปด", "เก้า", "สิบ" };
            string[] rank = { "", "สิบ", "ร้อย", "พัน", "หมื่น", "แสน", "ล้าน" };
            string[] temp = bahtTxt.Split('.');
            string intVal = temp[0];
            string decVal = temp[1];
            if (Convert.ToDouble(bahtTxt) == 0)
                bahtTH = "ศูนย์บาทถ้วน";
            else
            {
                for (int i = 0; i < intVal.Length; i++)
                {
                    n = intVal.Substring(i, 1);
                    if (n != "0")
                    {
                        if ((i == (intVal.Length - 1)) && (n == "1"))
                            bahtTH += "เอ็ด";
                        else if ((i == (intVal.Length - 2)) && (n == "2"))
                            bahtTH += "ยี่";
                        else if ((i == (intVal.Length - 2)) && (n == "1"))
                            bahtTH += "";
                        else
                            bahtTH += num[Convert.ToInt32(n)];
                        bahtTH += rank[(intVal.Length - i) - 1];
                    }
                }
                bahtTH += "บาท";
                if (decVal == "00")
                    bahtTH += "ถ้วน";
                else
                {
                    for (int i = 0; i < decVal.Length; i++)
                    {
                        n = decVal.Substring(i, 1);
                        if (n != "0")
                        {
                            if ((i == decVal.Length - 1) && (n == "1"))
                                bahtTH += "เอ็ด";
                            else if ((i == (decVal.Length - 2)) && (n == "2"))
                                bahtTH += "ยี่";
                            else if ((i == (decVal.Length - 2)) && (n == "1"))
                                bahtTH += "";
                            else
                                bahtTH += num[Convert.ToInt32(n)];
                            bahtTH += rank[(decVal.Length - i) - 1];
                        }
                    }
                    bahtTH += "สตางค์";
                }
            }
            return bahtTH;
        }

        public string FncGetMonthTH(string lvMonth)
        {
            string lvReturn = "";

            if (lvMonth == "01") lvReturn = "ม.ค.";
            else if (lvMonth == "02") lvReturn = "ก.พ.";
            else if (lvMonth == "03") lvReturn = "มี.ค.";
            else if (lvMonth == "04") lvReturn = "เม.ย.";
            else if (lvMonth == "05") lvReturn = "พ.ค.";
            else if (lvMonth == "06") lvReturn = "มิ.ย.";
            else if (lvMonth == "07") lvReturn = "ก.ค.";
            else if (lvMonth == "08") lvReturn = "ส.ค.";
            else if (lvMonth == "09") lvReturn = "ก.ย.";
            else if (lvMonth == "10") lvReturn = "ต.ค.";
            else if (lvMonth == "11") lvReturn = "พ.ย.";
            else if (lvMonth == "12") lvReturn = "ธ.ค.";

            return lvReturn;
        }

        public string FncGetDateOnly(string lvTXT)
        {
            string lvReturn = "";

            string[] lvArr = lvTXT.Split(' ');

            if (lvArr.Count() > 1)
            {
                lvReturn = lvArr[0];
            }
            else
            {
                lvReturn = lvTXT;
            }

            return lvReturn;
        }

        public string FncSetFormatDecimal(string lvTXT, int lvNumDecimal)
        {
            string lvReturn = "";

            string lvFormat = "";
            for (int i = 0; i < lvNumDecimal; i++)
            {
                lvFormat += "0";
            }

            double lvAmount = fncToDouble(lvTXT);
            lvReturn = lvAmount.ToString("#,###." + lvFormat);

            if (lvReturn == ".00" || lvReturn == ".000") lvReturn = "";

            return lvReturn;
        }

        public string FncGetYear(string lvYear)
        {
            string lvReturn = "";

            int lvYearN = fncToInt(lvYear);

            if (lvYearN > 2500) lvYearN -= 543;
            lvReturn = lvYearN.ToString();

            return lvReturn;
        }

        public string FncGetTimeOnly(string lvTime, string lvFormat)
        {
            string lvReturn = "";
            string[] tim = lvTime.Split(':');
            string tim1 = tim[0];

            if (tim1.Length == 1)
            {
                lvTime = "0" + tim1 + ":" + tim[1] + ":" + tim[2];
            }
            if (lvFormat == "HH:mm")
            {
                lvTime = Left(lvTime, 5);
                lvReturn = lvTime;
            }
            else if (lvFormat == "HH")
            {
                lvTime = Left(lvTime, 2);
                lvReturn = lvTime;
            }

            return lvReturn;
        }
    }
}
