using System.Collections.Generic;
using System.ComponentModel;

namespace MyMovies.BLL.Common
{
    public class LotPagedResults
    {
        /// <summary>
        /// The page number this page represents.
        /// </summary>
        [DefaultValue(1)]
        public int Page { get; set; }
        /// <summary>
        /// The size of this page.
        /// </summary>
        [DefaultValue(int.MaxValue)]
        public int Limit { get; set; }
        public bool AllLots { get; set; }
        [DefaultValue(0)]
        public int MinPrice { get; set; }
        [DefaultValue(int.MaxValue)]
        public int MaxPrice { get; set; }
        [DefaultValue(0)]
        public int MinYear { get; set; }
        [DefaultValue(int.MaxValue)]
        public int MaxYear { get; set; }
        [DefaultValue(0)]
        public int MinMileage { get; set; }
        [DefaultValue(int.MaxValue)]
        public int MaxMileage { get; set; }
        [DefaultValue(0)]
        public int MinPaintedDetails { get; set; }
        [DefaultValue(int.MaxValue)]
        public int MaxPaintedDetails { get; set; }
        [DefaultValue(0)]
        public int MinMark { get; set; }
        [DefaultValue(int.MaxValue)]
        public int MaxMark { get; set; }
        public int? NumericId { get; set; }
        public List<int> City { get; set; }
        public List<int> Color { get; set; }
        public List<int> ColorType { get; set; }
        public List<int> Body { get; set; }
        public List<int> DriveUnit { get; set; }
        public List<int> FuelType { get; set; }
        public List<int> TransmissionType { get; set; }
        public List<bool> InCredit { get; set; }
        public List<bool> AirConditionPresent { get; set; }
        [DefaultValue("Id.Ascending")]
        public string Order { get; set; }
        public List<string> CarMake { get; set; }
        public List<string> CarModel { get; set; }

        public LotPagedResults()
        {
            // Fallback value for empty query.
            MaxPrice = int.MaxValue;
            MaxYear = int.MaxValue;
            MaxMileage = int.MaxValue;
            MaxPaintedDetails = int.MaxValue;
            MaxMark = int.MaxValue;
            Color = new List<int>();
            ColorType = new List<int>();
            City = new List<int>();
            Body = new List<int>();
            DriveUnit = new List<int>();
            FuelType = new List<int>();
            TransmissionType = new List<int>();
            CarMake = new List<string>();
            CarModel = new List<string>();
            InCredit = new List<bool>();
            AirConditionPresent = new List<bool>();
            Page = 1;
            Limit = int.MaxValue;
            Order = "Id.Ascending";
        }
    }
}
