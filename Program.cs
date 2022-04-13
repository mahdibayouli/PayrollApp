using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CSProject
{
    class Staff
    {
        private float hourlyRate;
        private int hWorked;

        public Staff(string name, float rate)
        {
            this.NameOfStaff = name;
            this.hourlyRate = rate;
        }

            
        public float TotalPay { get  ; protected set ; }
        public string NameOfStaff { get  ; private set ; }
        public float BasicPay { get; private set; }
        public int HoursWorked {
            get {
                return hWorked;
            }
            set {
                if (value > 0)
                    hWorked = value;
                else hWorked = 0;
            }
        }

        public virtual void CalculatePay() {
            Console.WriteLine("Calculating Pay...");
            this.BasicPay = this.HoursWorked * this.hourlyRate;
            this.TotalPay = this.BasicPay;
        }

        public string toString() {
            return "\nhourlyRate: " + this.hourlyRate 
                + "\nTotalPay: " + this.TotalPay
                + "\nBasicPay: " + this.BasicPay
                + "\nNameOfStaff: " + this.NameOfStaff
                + "\nHoursWorked: " + this.HoursWorked;
        }
    }
    class Manager : Staff
    {
        private const float managerHourlyRate = 50f;

        public int Allowance { get; private set; }
        public Manager(string name) : base(name, managerHourlyRate){}
        public override void CalculatePay()
        {
            base.CalculatePay();
            Allowance = 1000;
            if (HoursWorked > 160) {
                TotalPay = BasicPay + Allowance;
            }
        }

        public override string ToString()
        {
            return base.ToString()
                + "\nmanagerHourlyRate: " + managerHourlyRate
                + "\nAllowance: " + Allowance;
        }
    }
    class Admin : Staff
    {
        private const float overtimeRate = 15.5f;
        private const float adminHourlyRate = 30f;

        public float Overtime { get; private set; }
        public Admin(string name) : base(name, adminHourlyRate){}

        public override void CalculatePay()
        {
            base.CalculatePay();
            if (HoursWorked > 160) {
                Overtime = overtimeRate * (HoursWorked - 160);
            }
        }

        public override string ToString()
        {
            return base.ToString()
                + "\novertimeRate: " + overtimeRate
                + "\nadminHourlyRate" + adminHourlyRate
                + "\nOvertime: " + Overtime;
        }
    }
    class FileReader
    {
        public List<Staff> ReadFile() {

            List<Staff> myStaff = new List<Staff>();
            string[] result = new string[2];
            string path = "staff.txt";
            string[] seperator = {", " };
            if (File.Exists(path))
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        result = line.Split(seperator, StringSplitOptions.RemoveEmptyEntries);
                        if (result[1].Equals("Manager"))
                        {
                            Manager manager = new Manager(result[0]);
                            myStaff.Add(manager);
                        }
                        else
                        {
                            Admin admin = new Admin(result[0]);
                            myStaff.Add(admin);
                        }
                    }
                    sr.Close();
                }
            }
            else {
                Console.WriteLine("File does not exist.");
            }

            return myStaff;
        }
    }
    class PaySlip
    {
        private int month;
        private int year;

        public PaySlip(int payMonth, int payYear)
        {
            this.month = payMonth;
            this.year = payYear;
        }

        enum MonthsOfYear {
            JAN = 1, FEB, MAR, APR, MAY, JUN, JUL, AUG, SEP, OCT, NOV, DEC 
        }
        


        public void GeneratePaySlip(List<Staff> myStaff) {
            string path;
            string equals = "==========================";
            foreach (Staff f in myStaff) {
                path = f.NameOfStaff+ ".txt";
                using (StreamWriter sw = new StreamWriter(path)) {
                /*1*/   sw.WriteLine("PAYSLIP FOR {0} {1}", (MonthsOfYear)month,year);
                /*2*/   sw.WriteLine(equals);
                /*3*/   sw.WriteLine("Name of Staff: {0}", f.NameOfStaff);
                /*4*/   sw.WriteLine("Hours Worked: {0}", f.HoursWorked);
                /*5*/   sw.WriteLine();
                /*6*/   sw.WriteLine("Basic Pay: {0:C}",f.BasicPay);
                /*7*/   sw.WriteLine("{0}: {1:C}", f.GetType() == typeof(Manager) ? "Allowance:" : "Overtime", f.GetType() == typeof(Manager)?((Manager)f).Allowance: ((Admin)f).Overtime);
                /*8*/   sw.WriteLine();
                /*9*/   sw.WriteLine(equals);
                /*10*/  sw.WriteLine("Total Pay: {0:C}", f.TotalPay);
                /*11*/  sw.WriteLine(equals);
                sw.Close();
                }
            }
        }
        public void GenerateSummary(List<Staff> myStaff) {
            var result = from staff in myStaff
                         where staff.HoursWorked < 10
                         orderby staff.NameOfStaff ascending
                         select new { staff.NameOfStaff, staff.HoursWorked};
            string path = "summary.txt";
            using (StreamWriter sw = new StreamWriter(path)) {
                sw.WriteLine("Staff with less than 10 working hours\n");
                foreach (var staff in result) {
                    sw.WriteLine("Name of Staff: {0}, Hours Worked: {1}",staff.NameOfStaff, staff.HoursWorked); 
                }
                sw.Close();
            }   
        }
        public override string ToString()
        {
            return base.ToString();
        }

    }

    internal class Program
    {
        static void Main(string[] args)
        {
            List<Staff> myStaff = new List<Staff>(); 
            FileReader fr = new FileReader();
            int month = 0, year = 0;
            while (year == 0) {
                Console.WriteLine("\nPlease enter the year: ");
                try
                {
                    //code to convert the input to an integer
                    year = Convert.ToInt32(Console.ReadLine());
                }
                catch (FormatException e ) {
                    //code to handle the exception 
                    Console.WriteLine(e.Message);
                }
            }

            while (month == 0)
            {
                Console.WriteLine("\nPlease enter the month: ");
                try
                {
                    //code to convert the input to an integer
                    month = Convert.ToInt32(Console.ReadLine());
                    if (month < 1 || month > 12) {
                        Console.WriteLine("month has to be between 1 and 12");
                        month = 0;
                        continue;
                    }
                }
                catch (FormatException e)
                {
                    //code to handle the exception 
                    Console.WriteLine(e.Message);
                }
            }
            myStaff = fr.ReadFile();
            for (int i = 0; i < myStaff.Count; i++)
            {
                try {
                    Console.WriteLine("Enter hours worked for {0}:", myStaff[i].NameOfStaff);
                    myStaff[i].HoursWorked = Convert.ToInt32(Console.ReadLine());
                    myStaff[i].CalculatePay();
                    Console.WriteLine(myStaff[i].toString());
                } 
                catch (Exception e) {
                    Console.WriteLine(e.Message);
                    i--;
                }
            }

            PaySlip ps = new PaySlip(month, year);
            ps.GeneratePaySlip(myStaff);
            ps.GenerateSummary(myStaff);
            Console.Read();



        }
    }

   
}
