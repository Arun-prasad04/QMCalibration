namespace WEB.Models
{
    public class DataTableParameters
    {

        public int iDisplayStart { get; set; }
        public int iDisplayLength { get; set; }
        public int iDisplayColumns { get; set; }
        public int iColumns { get; set; }
        public string sSearch { get; set; }
        public List<string> sColumnName { get; set; }

        public List<bool> bSortable { get; set; }

        public List<bool> bSearchable { get; set; }

        public List<string> sSearchValue { get; set; }

        public List<int> iSortCol { get; set; }
        public List<String> iSortDir { get; set; }

        public string sEcho { get; set; }
        public int reqType { get; set; }


    //    public DataTableParameters()
    //    {

        //        sColumnName = new List<string>();
        //        bSearchable = new List<bool>();
        //        bSortable = new List<bool>();
        //        sSearchValue = new List<string>();
        //        iSortDir = new List<string>();
        //        iSortCol= new List<int>();

        //    }

        //    public DataTableParameters(int iColumns)
        //    {
        //        this.iColumns = iColumns;
        //        sColumnName= new List<string>(iColumns);
        //        bSearchable = new List<bool>(iColumns);
        //        bSortable = new List<bool>(iColumns);
        //        sSearchValue = new List<string>(iColumns);
        //        iSortDir= new List<string>(iColumns);
        //        iSortCol= new List<int>(iColumns);
        //}
    }
}
