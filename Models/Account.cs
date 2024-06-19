using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ccsa_rfid_client.Models
{

    public class Account
    {
        public string AccId { get; set; }
        public string AttendanceId { get; set; }

        public string RfidTag { get; set;}

        public string CcsaId { get; set; }
        public string StdName { get; set; }
        public string Course { get; set; }
        public BitmapImage? Image { get; set; }

        internal Account(string accId, string attendanceId, string rfidTag, string ccsaId, string stdName, string course, BitmapImage? image = null)
        {
            AccId = accId;
            AttendanceId = attendanceId;
            RfidTag = rfidTag;
            CcsaId = ccsaId;
            StdName = stdName;
            Course = course;
            Image = image;
        }
    }
}
