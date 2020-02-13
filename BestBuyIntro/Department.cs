namespace BestBuyIntro
{
    internal class Department
    {
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }

        public Department(int currentDepartmentID, string currentDepartmentName)
        {
            DepartmentID = currentDepartmentID;
            DepartmentName = currentDepartmentName;
        }

        public Department() { }
    }
}