document.addEventListener('DOMContentLoaded', function() {
    // Khai báo các dropdown
    const provinceSelect = document.getElementById('province');
    const districtSelect = document.getElementById('district');
    
    // Disable dropdown quận/huyện ban đầu
    districtSelect.disabled = true;
    
    // Dữ liệu tỉnh/thành phố (63 tỉnh thành Việt Nam)
    const provinces = [
        { code: "01", name: "Hà Nội" },
        { code: "02", name: "Hà Giang" },
        { code: "04", name: "Cao Bằng" },
        { code: "06", name: "Bắc Kạn" },
        { code: "08", name: "Tuyên Quang" },
        { code: "10", name: "Lào Cai" },
        { code: "11", name: "Điện Biên" },
        { code: "12", name: "Lai Châu" },
        { code: "14", name: "Sơn La" },
        { code: "15", name: "Yên Bái" },
        { code: "17", name: "Hoà Bình" },
        { code: "19", name: "Thái Nguyên" },
        { code: "20", name: "Lạng Sơn" },
        { code: "22", name: "Quảng Ninh" },
        { code: "24", name: "Bắc Giang" },
        { code: "25", name: "Phú Thọ" },
        { code: "26", name: "Vĩnh Phúc" },
        { code: "27", name: "Bắc Ninh" },
        { code: "30", name: "Hải Dương" },
        { code: "31", name: "Hải Phòng" },
        { code: "33", name: "Hưng Yên" },
        { code: "34", name: "Thái Bình" },
        { code: "35", name: "Hà Nam" },
        { code: "36", name: "Nam Định" },
        { code: "37", name: "Ninh Bình" },
        { code: "38", name: "Thanh Hóa" },
        { code: "40", name: "Nghệ An" },
        { code: "42", name: "Hà Tĩnh" },
        { code: "44", name: "Quảng Bình" },
        { code: "45", name: "Quảng Trị" },
        { code: "46", name: "Thừa Thiên Huế" },
        { code: "48", name: "Đà Nẵng" },
        { code: "49", name: "Quảng Nam" },
        { code: "51", name: "Quảng Ngãi" },
        { code: "52", name: "Bình Định" },
        { code: "54", name: "Phú Yên" },
        { code: "56", name: "Khánh Hòa" },
        { code: "58", name: "Ninh Thuận" },
        { code: "60", name: "Bình Thuận" },
        { code: "62", name: "Kon Tum" },
        { code: "64", name: "Gia Lai" },
        { code: "66", name: "Đắk Lắk" },
        { code: "67", name: "Đắk Nông" },
        { code: "68", name: "Lâm Đồng" },
        { code: "70", name: "Bình Phước" },
        { code: "72", name: "Tây Ninh" },
        { code: "74", name: "Bình Dương" },
        { code: "75", name: "Đồng Nai" },
        { code: "77", name: "Bà Rịa - Vũng Tàu" },
        { code: "79", name: "Hồ Chí Minh" },
        { code: "80", name: "Long An" },
        { code: "82", name: "Tiền Giang" },
        { code: "83", name: "Bến Tre" },
        { code: "84", name: "Trà Vinh" },
        { code: "86", name: "Vĩnh Long" },
        { code: "87", name: "Đồng Tháp" },
        { code: "89", name: "An Giang" },
        { code: "91", name: "Kiên Giang" },
        { code: "92", name: "Cần Thơ" },
        { code: "93", name: "Hậu Giang" },
        { code: "94", name: "Sóc Trăng" },
        { code: "95", name: "Bạc Liêu" },
        { code: "96", name: "Cà Mau" }
    ];
    
    // Một vài huyện điển hình theo tỉnh
    const districtsByProvince = {
        // Hà Nội
        "01": [
            { code: "001", name: "Ba Đình" },
            { code: "002", name: "Hoàn Kiếm" },
            { code: "003", name: "Tây Hồ" },
            { code: "004", name: "Long Biên" },
            { code: "005", name: "Cầu Giấy" },
            { code: "006", name: "Đống Đa" },
            { code: "007", name: "Hai Bà Trưng" },
            { code: "008", name: "Hoàng Mai" },
            { code: "009", name: "Thanh Xuân" },
            { code: "016", name: "Sóc Sơn" },
            { code: "017", name: "Đông Anh" },
            { code: "018", name: "Gia Lâm" },
            { code: "019", name: "Nam Từ Liêm" },
            { code: "020", name: "Thanh Trì" },
            { code: "021", name: "Bắc Từ Liêm" }
        ],
        // Hà Giang
        "02": [
            { code: "024", name: "Thành phố Hà Giang" },
            { code: "026", name: "Huyện Đồng Văn" },
            { code: "027", name: "Huyện Mèo Vạc" },
            { code: "028", name: "Huyện Yên Minh" },
            { code: "029", name: "Huyện Quản Bạ" },
            { code: "030", name: "Huyện Vị Xuyên" },
            { code: "031", name: "Huyện Bắc Mê" },
            { code: "032", name: "Huyện Hoàng Su Phì" },
            { code: "033", name: "Huyện Xín Mần" },
            { code: "034", name: "Huyện Bắc Quang" },
            { code: "035", name: "Huyện Quang Bình" }
        ],
        // Cao Bằng
        "04": [
            { code: "040", name: "Thành phố Cao Bằng" },
            { code: "042", name: "Huyện Bảo Lâm" },
            { code: "043", name: "Huyện Bảo Lạc" },
            { code: "044", name: "Huyện Thông Nông" },
            { code: "045", name: "Huyện Hà Quảng" },
            { code: "046", name: "Huyện Trà Lĩnh" },
            { code: "047", name: "Huyện Trùng Khánh" },
            { code: "048", name: "Huyện Hạ Lang" },
            { code: "049", name: "Huyện Quảng Uyên" },
            { code: "050", name: "Huyện Phục Hoà" },
            { code: "051", name: "Huyện Hoà An" },
            { code: "052", name: "Huyện Nguyên Bình" },
            { code: "053", name: "Huyện Thạch An" }
        ],
        // Bắc Kạn
        "06": [
            { code: "058", name: "Thành Phố Bắc Kạn" },
            { code: "060", name: "Huyện Pác Nặm" },
            { code: "061", name: "Huyện Ba Bể" },
            { code: "062", name: "Huyện Ngân Sơn" },
            { code: "063", name: "Huyện Bạch Thông" },
            { code: "064", name: "Huyện Chợ Đồn" },
            { code: "065", name: "Huyện Chợ Mới" },
            { code: "066", name: "Huyện Na Rì" }
        ],
        // Tuyên Quang
        "08": [
            { code: "070", name: "Thành phố Tuyên Quang" },
            { code: "071", name: "Huyện Lâm Bình" },
            { code: "072", name: "Huyện Na Hang" },
            { code: "073", name: "Huyện Chiêm Hóa" },
            { code: "074", name: "Huyện Hàm Yên" },
            { code: "075", name: "Huyện Yên Sơn" },
            { code: "076", name: "Huyện Sơn Dương" }
        ],
        // Lào Cai
        "10": [
            { code: "080", name: "Thành phố Lào Cai" },
            { code: "082", name: "Huyện Bát Xát" },
            { code: "083", name: "Huyện Mường Khương" },
            { code: "084", name: "Huyện Si Ma Cai" },
            { code: "085", name: "Huyện Bắc Hà" },
            { code: "086", name: "Huyện Bảo Thắng" },
            { code: "087", name: "Huyện Bảo Yên" },
            { code: "088", name: "Huyện Sa Pa" },
            { code: "089", name: "Huyện Văn Bàn" }
        ],
        // Điện Biên
        "11": [
            { code: "094", name: "Thành phố Điện Biên Phủ" },
            { code: "095", name: "Thị Xã Mường Lay" },
            { code: "096", name: "Huyện Mường Nhé" },
            { code: "097", name: "Huyện Mường Chà" },
            { code: "098", name: "Huyện Tủa Chùa" },
            { code: "099", name: "Huyện Tuần Giáo" },
            { code: "100", name: "Huyện Điện Biên" },
            { code: "101", name: "Huyện Điện Biên Đông" },
            { code: "102", name: "Huyện Mường Ảng" },
            { code: "103", name: "Huyện Nậm Pồ" }
        ],
        // Lai Châu
        "12": [
            { code: "105", name: "Thành phố Lai Châu" },
            { code: "106", name: "Huyện Tam Đường" },
            { code: "107", name: "Huyện Mường Tè" },
            { code: "108", name: "Huyện Sìn Hồ" },
            { code: "109", name: "Huyện Phong Thổ" },
            { code: "110", name: "Huyện Than Uyên" },
            { code: "111", name: "Huyện Tân Uyên" },
            { code: "112", name: "Huyện Nậm Nhùn" }
        ],
        // Sơn La
        "14": [
            { code: "116", name: "Thành phố Sơn La" },
            { code: "118", name: "Huyện Quỳnh Nhai" },
            { code: "119", name: "Huyện Thuận Châu" },
            { code: "120", name: "Huyện Mường La" },
            { code: "121", name: "Huyện Bắc Yên" },
            { code: "122", name: "Huyện Phù Yên" },
            { code: "123", name: "Huyện Mộc Châu" },
            { code: "124", name: "Huyện Yên Châu" },
            { code: "125", name: "Huyện Mai Sơn" },
            { code: "126", name: "Huyện Sông Mã" },
            { code: "127", name: "Huyện Sốp Cộp" },
            { code: "128", name: "Huyện Vân Hồ" }
        ],
        // Yên Bái
        "15": [
            { code: "132", name: "Thành phố Yên Bái" },
            { code: "133", name: "Thị xã Nghĩa Lộ" },
            { code: "135", name: "Huyện Lục Yên" },
            { code: "136", name: "Huyện Văn Yên" },
            { code: "137", name: "Huyện Mù Căng Chải" },
            { code: "138", name: "Huyện Trấn Yên" },
            { code: "139", name: "Huyện Trạm Tấu" },
            { code: "140", name: "Huyện Văn Chấn" },
            { code: "141", name: "Huyện Yên Bình" }
        ],
        // Hoà Bình
        "17": [
            { code: "148", name: "Thành phố Hòa Bình" },
            { code: "150", name: "Huyện Đà Bắc" },
            { code: "151", name: "Huyện Kỳ Sơn" },
            { code: "152", name: "Huyện Lương Sơn" },
            { code: "153", name: "Huyện Kim Bôi" },
            { code: "154", name: "Huyện Cao Phong" },
            { code: "155", name: "Huyện Tân Lạc" },
            { code: "156", name: "Huyện Mai Châu" },
            { code: "157", name: "Huyện Lạc Sơn" },
            { code: "158", name: "Huyện Yên Thủy" },
            { code: "159", name: "Huyện Lạc Thủy" }
        ],
        // Thái Nguyên
        "19": [
            { code: "164", name: "Thành phố Thái Nguyên" },
            { code: "165", name: "Thành phố Sông Công" },
            { code: "167", name: "Huyện Định Hóa" },
            { code: "168", name: "Huyện Phú Lương" },
            { code: "169", name: "Huyện Đồng Hỷ" },
            { code: "170", name: "Huyện Võ Nhai" },
            { code: "171", name: "Huyện Đại Từ" },
            { code: "172", name: "Thị xã Phổ Yên" },
            { code: "173", name: "Huyện Phú Bình" }
        ],
        // Lạng Sơn
        "20": [
            { code: "178", name: "Thành phố Lạng Sơn" },
            { code: "180", name: "Huyện Tràng Định" },
            { code: "181", name: "Huyện Bình Gia" },
            { code: "182", name: "Huyện Văn Lãng" },
            { code: "183", name: "Huyện Cao Lộc" },
            { code: "184", name: "Huyện Văn Quan" },
            { code: "185", name: "Huyện Bắc Sơn" },
            { code: "186", name: "Huyện Hữu Lũng" },
            { code: "187", name: "Huyện Chi Lăng" },
            { code: "188", name: "Huyện Lộc Bình" },
            { code: "189", name: "Huyện Đình Lập" }
        ],
        // Quảng Ninh
        "22": [
            { code: "193", name: "Thành phố Hạ Long" },
            { code: "194", name: "Thành phố Móng Cái" },
            { code: "195", name: "Thành phố Cẩm Phả" },
            { code: "196", name: "Thành phố Uông Bí" },
            { code: "198", name: "Huyện Bình Liêu" },
            { code: "199", name: "Huyện Tiên Yên" },
            { code: "200", name: "Huyện Đầm Hà" },
            { code: "201", name: "Huyện Hải Hà" },
            { code: "202", name: "Huyện Ba Chẽ" },
            { code: "203", name: "Huyện Vân Đồn" },
            { code: "204", name: "Thị xã Đông Triều" },
            { code: "205", name: "Thị xã Quảng Yên" },
            { code: "206", name: "Huyện Cô Tô" }
        ],
        // Bắc Giang
        "24": [
            { code: "213", name: "Thành phố Bắc Giang" },
            { code: "215", name: "Huyện Yên Thế" },
            { code: "216", name: "Huyện Tân Yên" },
            { code: "217", name: "Huyện Lạng Giang" },
            { code: "218", name: "Huyện Lục Nam" },
            { code: "219", name: "Huyện Lục Ngạn" },
            { code: "220", name: "Huyện Sơn Động" },
            { code: "221", name: "Huyện Yên Dũng" },
            { code: "222", name: "Huyện Việt Yên" },
            { code: "223", name: "Huyện Hiệp Hòa" }
        ],
        // Phú Thọ
        "25": [
            { code: "227", name: "Thành phố Việt Trì" },
            { code: "228", name: "Thị xã Phú Thọ" },
            { code: "230", name: "Huyện Đoan Hùng" },
            { code: "231", name: "Huyện Hạ Hoà" },
            { code: "232", name: "Huyện Thanh Ba" },
            { code: "233", name: "Huyện Phù Ninh" },
            { code: "234", name: "Huyện Yên Lập" },
            { code: "235", name: "Huyện Cẩm Khê" },
            { code: "236", name: "Huyện Tam Nông" },
            { code: "237", name: "Huyện Lâm Thao" },
            { code: "238", name: "Huyện Thanh Sơn" },
            { code: "239", name: "Huyện Thanh Thuỷ" },
            { code: "240", name: "Huyện Tân Sơn" }
        ],
        // Vĩnh Phúc
        "26": [
            { code: "243", name: "Thành phố Vĩnh Yên" },
            { code: "244", name: "Thành phố Phúc Yên" },
            { code: "246", name: "Huyện Lập Thạch" },
            { code: "247", name: "Huyện Tam Dương" },
            { code: "248", name: "Huyện Tam Đảo" },
            { code: "249", name: "Huyện Bình Xuyên" },
            { code: "250", name: "Huyện Mê Linh" },
            { code: "251", name: "Huyện Yên Lạc" },
            { code: "252", name: "Huyện Vĩnh Tường" },
            { code: "253", name: "Huyện Sông Lô" }
        ],
        // Bắc Ninh
        "27": [
            { code: "256", name: "Thành phố Bắc Ninh" },
            { code: "258", name: "Huyện Yên Phong" },
            { code: "259", name: "Huyện Quế Võ" },
            { code: "260", name: "Huyện Tiên Du" },
            { code: "261", name: "Thị xã Từ Sơn" },
            { code: "262", name: "Huyện Thuận Thành" },
            { code: "263", name: "Huyện Gia Bình" },
            { code: "264", name: "Huyện Lương Tài" }
        ],
        // Hải Dương
        "30": [
            { code: "288", name: "Thành phố Hải Dương" },
            { code: "290", name: "Thành phố Chí Linh" },
            { code: "291", name: "Huyện Nam Sách" },
            { code: "292", name: "Thị xã Kinh Môn" },
            { code: "293", name: "Huyện Kim Thành" },
            { code: "294", name: "Huyện Thanh Hà" },
            { code: "295", name: "Huyện Cẩm Giàng" },
            { code: "296", name: "Huyện Bình Giang" },
            { code: "297", name: "Huyện Gia Lộc" },
            { code: "298", name: "Huyện Tứ Kỳ" },
            { code: "299", name: "Huyện Ninh Giang" },
            { code: "300", name: "Huyện Thanh Miện" }
        ],
        // Hải Phòng
        "31": [
            { code: "303", name: "Quận Hồng Bàng" },
            { code: "304", name: "Quận Ngô Quyền" },
            { code: "305", name: "Quận Lê Chân" },
            { code: "306", name: "Quận Hải An" },
            { code: "307", name: "Quận Kiến An" },
            { code: "308", name: "Quận Đồ Sơn" },
            { code: "309", name: "Quận Dương Kinh" },
            { code: "311", name: "Huyện Thuỷ Nguyên" },
            { code: "312", name: "Huyện An Dương" },
            { code: "313", name: "Huyện An Lão" },
            { code: "314", name: "Huyện Kiến Thuỵ" },
            { code: "315", name: "Huyện Tiên Lãng" },
            { code: "316", name: "Huyện Vĩnh Bảo" },
            { code: "317", name: "Huyện Cát Hải" },
            { code: "318", name: "Huyện Bạch Long Vĩ" }
        ],
        // Hưng Yên
        "33": [
            { code: "323", name: "Thành phố Hưng Yên" },
            { code: "325", name: "Huyện Văn Lâm" },
            { code: "326", name: "Huyện Văn Giang" },
            { code: "327", name: "Huyện Yên Mỹ" },
            { code: "328", name: "Thị xã Mỹ Hào" },
            { code: "329", name: "Huyện Ân Thi" },
            { code: "330", name: "Huyện Khoái Châu" },
            { code: "331", name: "Huyện Kim Động" },
            { code: "332", name: "Huyện Tiên Lữ" },
            { code: "333", name: "Huyện Phù Cừ" }
        ],
        // Thái Bình
        "34": [
            { code: "336", name: "Thành phố Thái Bình" },
            { code: "338", name: "Huyện Quỳnh Phụ" },
            { code: "339", name: "Huyện Hưng Hà" },
            { code: "340", name: "Huyện Đông Hưng" },
            { code: "341", name: "Huyện Thái Thụy" },
            { code: "342", name: "Huyện Tiền Hải" },
            { code: "343", name: "Huyện Kiến Xương" },
            { code: "344", name: "Huyện Vũ Thư" }
        ],
        // Hà Nam
        "35": [
            { code: "347", name: "Thành phố Phủ Lý" },
            { code: "349", name: "Thị xã Duy Tiên" },
            { code: "350", name: "Huyện Kim Bảng" },
            { code: "351", name: "Huyện Thanh Liêm" },
            { code: "352", name: "Huyện Bình Lục" },
            { code: "353", name: "Huyện Lý Nhân" }
        ],
        // Nam Định
        "36": [
            { code: "356", name: "Thành phố Nam Định" },
            { code: "358", name: "Huyện Mỹ Lộc" },
            { code: "359", name: "Huyện Vụ Bản" },
            { code: "360", name: "Huyện Ý Yên" },
            { code: "361", name: "Huyện Nghĩa Hưng" },
            { code: "362", name: "Huyện Nam Trực" },
            { code: "363", name: "Huyện Trực Ninh" },
            { code: "364", name: "Huyện Xuân Trường" },
            { code: "365", name: "Huyện Giao Thủy" },
            { code: "366", name: "Huyện Hải Hậu" }
        ],
        // Ninh Bình
        "37": [
            { code: "369", name: "Thành phố Ninh Bình" },
            { code: "370", name: "Thành phố Tam Điệp" },
            { code: "372", name: "Huyện Nho Quan" },
            { code: "373", name: "Huyện Gia Viễn" },
            { code: "374", name: "Huyện Hoa Lư" },
            { code: "375", name: "Huyện Yên Khánh" },
            { code: "376", name: "Huyện Kim Sơn" },
            { code: "377", name: "Huyện Yên Mô" }
        ],
        // Thanh Hóa
        "38": [
            { code: "380", name: "Thành phố Thanh Hóa" },
            { code: "381", name: "Thị xã Bỉm Sơn" },
            { code: "382", name: "Thành phố Sầm Sơn" },
            { code: "384", name: "Huyện Mường Lát" },
            { code: "385", name: "Huyện Quan Hóa" },
            { code: "386", name: "Huyện Bá Thước" },
            { code: "387", name: "Huyện Quan Sơn" },
            { code: "388", name: "Huyện Lang Chánh" },
            { code: "389", name: "Huyện Ngọc Lặc" },
            { code: "390", name: "Huyện Cẩm Thủy" },
            { code: "391", name: "Huyện Thạch Thành" },
            { code: "392", name: "Huyện Hà Trung" },
            { code: "393", name: "Huyện Vĩnh Lộc" },
            { code: "394", name: "Huyện Yên Định" },
            { code: "395", name: "Huyện Thọ Xuân" },
            { code: "396", name: "Huyện Thường Xuân" },
            { code: "397", name: "Huyện Triệu Sơn" },
            { code: "398", name: "Huyện Thiệu Hóa" },
            { code: "399", name: "Huyện Hoằng Hóa" },
            { code: "400", name: "Huyện Hậu Lộc" },
            { code: "401", name: "Huyện Nga Sơn" },
            { code: "402", name: "Huyện Như Xuân" },
            { code: "403", name: "Huyện Như Thanh" },
            { code: "404", name: "Huyện Nông Cống" },
            { code: "405", name: "Huyện Đông Sơn" },
            { code: "406", name: "Huyện Quảng Xương" },
            { code: "407", name: "Thị xã Nghi Sơn" }
        ],
        // Nghệ An
        "40": [
            { code: "412", name: "Thành phố Vinh" },
            { code: "413", name: "Thị xã Cửa Lò" },
            { code: "414", name: "Thị xã Thái Hoà" },
            { code: "415", name: "Huyện Quế Phong" },
            { code: "416", name: "Huyện Quỳ Châu" },
            { code: "417", name: "Huyện Kỳ Sơn" },
            { code: "418", name: "Huyện Tương Dương" },
            { code: "419", name: "Huyện Nghĩa Đàn" },
            { code: "420", name: "Huyện Quỳ Hợp" },
            { code: "421", name: "Huyện Quỳnh Lưu" },
            { code: "422", name: "Huyện Con Cuông" },
            { code: "423", name: "Huyện Tân Kỳ" },
            { code: "424", name: "Huyện Anh Sơn" },
            { code: "425", name: "Huyện Diễn Châu" },
            { code: "426", name: "Huyện Yên Thành" },
            { code: "427", name: "Huyện Đô Lương" },
            { code: "428", name: "Huyện Thanh Chương" },
            { code: "429", name: "Huyện Nghi Lộc" },
            { code: "430", name: "Huyện Nam Đàn" },
            { code: "431", name: "Huyện Hưng Nguyên" },
            { code: "432", name: "Thị xã Hoàng Mai" }
        ],
        // Hà Tĩnh
        "42": [
            { code: "436", name: "Thành phố Hà Tĩnh" },
            { code: "437", name: "Thị xã Hồng Lĩnh" },
            { code: "439", name: "Huyện Hương Sơn" },
            { code: "440", name: "Huyện Đức Thọ" },
            { code: "441", name: "Huyện Vũ Quang" },
            { code: "442", name: "Huyện Nghi Xuân" },
            { code: "443", name: "Huyện Can Lộc" },
            { code: "444", name: "Huyện Hương Khê" },
            { code: "445", name: "Huyện Thạch Hà" },
            { code: "446", name: "Huyện Cẩm Xuyên" },
            { code: "447", name: "Huyện Kỳ Anh" },
            { code: "448", name: "Huyện Lộc Hà" },
            { code: "449", name: "Thị xã Kỳ Anh" }
        ],
        // Quảng Bình
        "44": [
            { code: "450", name: "Thành Phố Đồng Hới" },
            { code: "452", name: "Huyện Minh Hóa" },
            { code: "453", name: "Huyện Tuyên Hóa" },
            { code: "454", name: "Huyện Quảng Trạch" },
            { code: "455", name: "Huyện Bố Trạch" },
            { code: "456", name: "Huyện Quảng Ninh" },
            { code: "457", name: "Huyện Lệ Thủy" },
            { code: "458", name: "Thị xã Ba Đồn" }
        ],
        // Quảng Trị
        "45": [
            { code: "461", name: "Thành phố Đông Hà" },
            { code: "462", name: "Thị xã Quảng Trị" },
            { code: "464", name: "Huyện Vĩnh Linh" },
            { code: "465", name: "Huyện Hướng Hóa" },
            { code: "466", name: "Huyện Gio Linh" },
            { code: "467", name: "Huyện Đakrông" },
            { code: "468", name: "Huyện Cam Lộ" },
            { code: "469", name: "Huyện Triệu Phong" },
            { code: "470", name: "Huyện Hải Lăng" },
            { code: "471", name: "Huyện Cồn Cỏ" }
        ],
        // Thừa Thiên Huế
        "46": [
            { code: "474", name: "Thành phố Huế" },
            { code: "476", name: "Huyện Phong Điền" },
            { code: "477", name: "Huyện Quảng Điền" },
            { code: "478", name: "Huyện Phú Vang" },
            { code: "479", name: "Thị xã Hương Thủy" },
            { code: "480", name: "Thị xã Hương Trà" },
            { code: "481", name: "Huyện A Lưới" },
            { code: "482", name: "Huyện Phú Lộc" },
            { code: "483", name: "Huyện Nam Đông" }
        ],
        // Đà Nẵng
        "48": [
            { code: "490", name: "Quận Liên Chiểu" },
            { code: "491", name: "Quận Thanh Khê" },
            { code: "492", name: "Quận Hải Châu" },
            { code: "493", name: "Quận Sơn Trà" },
            { code: "494", name: "Quận Ngũ Hành Sơn" },
            { code: "495", name: "Quận Cẩm Lệ" },
            { code: "497", name: "Huyện Hòa Vang" },
            { code: "498", name: "Huyện Hoàng Sa" }
        ],
        // Quảng Nam
        "49": [
            { code: "502", name: "Thành phố Tam Kỳ" },
            { code: "503", name: "Thành phố Hội An" },
            { code: "504", name: "Huyện Tây Giang" },
            { code: "505", name: "Huyện Đông Giang" },
            { code: "506", name: "Huyện Đại Lộc" },
            { code: "507", name: "Thị xã Điện Bàn" },
            { code: "508", name: "Huyện Duy Xuyên" },
            { code: "509", name: "Huyện Quế Sơn" },
            { code: "510", name: "Huyện Nam Giang" },
            { code: "511", name: "Huyện Phước Sơn" },
            { code: "512", name: "Huyện Hiệp Đức" },
            { code: "513", name: "Huyện Thăng Bình" },
            { code: "514", name: "Huyện Tiên Phước" },
            { code: "515", name: "Huyện Bắc Trà My" },
            { code: "516", name: "Huyện Nam Trà My" },
            { code: "517", name: "Huyện Núi Thành" },
            { code: "518", name: "Huyện Phú Ninh" },
            { code: "519", name: "Huyện Nông Sơn" }
        ],
        // Quảng Ngãi
        "51": [
            { code: "522", name: "Thành phố Quảng Ngãi" },
            { code: "524", name: "Huyện Bình Sơn" },
            { code: "525", name: "Huyện Trà Bồng" },
            { code: "526", name: "Huyện Tây Trà" },
            { code: "527", name: "Huyện Sơn Tịnh" },
            { code: "528", name: "Huyện Tư Nghĩa" },
            { code: "529", name: "Huyện Sơn Hà" },
            { code: "530", name: "Huyện Sơn Tây" },
            { code: "531", name: "Huyện Minh Long" },
            { code: "532", name: "Huyện Nghĩa Hành" },
            { code: "533", name: "Huyện Mộ Đức" },
            { code: "534", name: "Thị xã Đức Phổ" },
            { code: "535", name: "Huyện Ba Tơ" },
            { code: "536", name: "Huyện Lý Sơn" }
        ],
        // Bình Định
        "52": [
            { code: "540", name: "Thành phố Quy Nhơn" },
            { code: "542", name: "Huyện An Lão" },
            { code: "543", name: "Thị xã Hoài Nhơn" },
            { code: "544", name: "Huyện Hoài Ân" },
            { code: "545", name: "Huyện Phù Mỹ" },
            { code: "546", name: "Huyện Vĩnh Thạnh" },
            { code: "547", name: "Huyện Tây Sơn" },
            { code: "548", name: "Huyện Phù Cát" },
            { code: "549", name: "Thị xã An Nhơn" },
            { code: "550", name: "Huyện Tuy Phước" },
            { code: "551", name: "Huyện Vân Canh" }
        ],
        // Phú Yên
        "54": [
            { code: "555", name: "Thành phố Tuy Hoà" },
            { code: "557", name: "Thị xã Sông Cầu" },
            { code: "558", name: "Huyện Đồng Xuân" },
            { code: "559", name: "Huyện Tuy An" },
            { code: "560", name: "Huyện Sơn Hòa" },
            { code: "561", name: "Huyện Sông Hinh" },
            { code: "562", name: "Huyện Tây Hoà" },
            { code: "563", name: "Huyện Phú Hoà" },
            { code: "564", name: "Thị xã Đông Hòa" }
        ],
        // Khánh Hòa
        "56": [
            { code: "568", name: "Thành phố Nha Trang" },
            { code: "569", name: "Thành phố Cam Ranh" },
            { code: "570", name: "Huyện Cam Lâm" },
            { code: "571", name: "Huyện Vạn Ninh" },
            { code: "572", name: "Thị xã Ninh Hòa" },
            { code: "573", name: "Huyện Khánh Vĩnh" },
            { code: "574", name: "Huyện Diên Khánh" },
            { code: "575", name: "Huyện Khánh Sơn" },
            { code: "576", name: "Huyện Trường Sa" }
        ],
        // Ninh Thuận
        "58": [
            { code: "582", name: "Thành phố Phan Rang-Tháp Chàm" },
            { code: "584", name: "Huyện Bác Ái" },
            { code: "585", name: "Huyện Ninh Sơn" },
            { code: "586", name: "Huyện Ninh Hải" },
            { code: "587", name: "Huyện Ninh Phước" },
            { code: "588", name: "Huyện Thuận Bắc" },
            { code: "589", name: "Huyện Thuận Nam" }
        ],
        // Bình Thuận
        "60": [
            { code: "593", name: "Thành phố Phan Thiết" },
            { code: "594", name: "Thị xã La Gi" },
            { code: "595", name: "Huyện Tuy Phong" },
            { code: "596", name: "Huyện Bắc Bình" },
            { code: "597", name: "Huyện Hàm Thuận Bắc" },
            { code: "598", name: "Huyện Hàm Thuận Nam" },
            { code: "599", name: "Huyện Tánh Linh" },
            { code: "600", name: "Huyện Đức Linh" },
            { code: "601", name: "Huyện Hàm Tân" },
            { code: "602", name: "Huyện Phú Quí" }
        ],
        // Kon Tum
        "62": [
            { code: "608", name: "Thành phố Kon Tum" },
            { code: "610", name: "Huyện Đắk Glei" },
            { code: "611", name: "Huyện Ngọc Hồi" },
            { code: "612", name: "Huyện Đắk Tô" },
            { code: "613", name: "Huyện Kon Plông" },
            { code: "614", name: "Huyện Kon Rẫy" },
            { code: "615", name: "Huyện Đắk Hà" },
            { code: "616", name: "Huyện Sa Thầy" },
            { code: "617", name: "Huyện Tu Mơ Rông" },
            { code: "618", name: "Huyện Ia H' Drai" }
        ],
        // Gia Lai
        "64": [
            { code: "622", name: "Thành phố Pleiku" },
            { code: "623", name: "Thị xã Buôn Hồ" },
            { code: "624", name: "Huyện Ea H'leo" },
            { code: "625", name: "Huyện Ea Súp" },
            { code: "626", name: "Huyện Buôn Đôn" },
            { code: "627", name: "Huyện Cư M'gar" },
            { code: "628", name: "Huyện Krông Búk" },
            { code: "629", name: "Huyện Krông Năng" },
            { code: "630", name: "Huyện Ea Kar" },
            { code: "631", name: "Huyện M'Đrắk" },
            { code: "632", name: "Huyện Krông Bông" },
            { code: "633", name: "Huyện Krông Pắc" },
            { code: "634", name: "Huyện Krông A Na" },
            { code: "635", name: "Huyện Lắk" },
            { code: "637", name: "Huyện Krông Pa" },
            { code: "638", name: "Huyện Phú Thiện" },
            { code: "639", name: "Huyện Chư Pưh" }
        ],
        // Đắk Lắk
        "66": [
            { code: "643", name: "Thành phố Buôn Ma Thuột" },
            { code: "644", name: "Thị xã Buôn Hồ" },
            { code: "645", name: "Huyện Ea H'leo" },
            { code: "646", name: "Huyện Ea Súp" },
            { code: "647", name: "Huyện Buôn Đôn" },
            { code: "648", name: "Huyện Cư M'gar" },
            { code: "649", name: "Huyện Krông Búk" },
            { code: "650", name: "Huyện Krông Năng" },
            { code: "651", name: "Huyện Ea Kar" },
            { code: "652", name: "Huyện M'Đrắk" },
            { code: "653", name: "Huyện Krông Bông" },
            { code: "654", name: "Huyện Krông Pắc" },
            { code: "655", name: "Huyện Krông A Na" },
            { code: "656", name: "Huyện Lắk" },
            { code: "657", name: "Huyện Cư Kuin" }
        ],
        // Đắk Nông
        "67": [
            { code: "660", name: "Thành phố Gia Nghĩa" },
            { code: "661", name: "Huyện Đăk Glong" },
            { code: "662", name: "Huyện Cư Jút" },
            { code: "663", name: "Huyện Đắk Mil" },
            { code: "664", name: "Huyện Krông Nô" },
            { code: "665", name: "Huyện Đắk Song" },
            { code: "666", name: "Huyện Đắk R'Lấp" },
            { code: "667", name: "Huyện Tuy Đức" }
        ],
        // Lâm Đồng
        "68": [
            { code: "672", name: "Thành phố Đà Lạt" },
            { code: "673", name: "Thành phố Bảo Lộc" },
            { code: "674", name: "Huyện Đam Rông" },
            { code: "675", name: "Huyện Lạc Dương" },
            { code: "676", name: "Huyện Lâm Hà" },
            { code: "677", name: "Huyện Đơn Dương" },
            { code: "678", name: "Huyện Đức Trọng" },
            { code: "679", name: "Huyện Di Linh" },
            { code: "680", name: "Huyện Bảo Lâm" },
            { code: "681", name: "Huyện Đạ Huoai" },
            { code: "682", name: "Huyện Đạ Tẻh" },
            { code: "683", name: "Huyện Cát Tiên" }
        ],
        // Bình Phước
        "70": [
            { code: "688", name: "Thành phố Đồng Xoài" },
            { code: "689", name: "Thị xã Phước Long" },
            { code: "690", name: "Thị xã Bình Long" },
            { code: "691", name: "Huyện Bù Gia Mập" },
            { code: "692", name: "Huyện Lộc Ninh" },
            { code: "693", name: "Huyện Bù Đốp" },
            { code: "694", name: "Huyện Hớn Quản" },
            { code: "695", name: "Huyện Đồng Phú" },
            { code: "696", name: "Huyện Bù Đăng" },
            { code: "697", name: "Huyện Chơn Thành" },
            { code: "698", name: "Huyện Phú Riềng" }
        ],
        // Tây Ninh
        "72": [
            { code: "703", name: "Thành phố Tây Ninh" },
            { code: "705", name: "Huyện Tân Biên" },
            { code: "706", name: "Huyện Tân Châu" },
            { code: "707", name: "Huyện Dương Minh Châu" },
            { code: "708", name: "Huyện Châu Thành" },
            { code: "709", name: "Huyện Hòa Thành" },
            { code: "710", name: "Huyện Gò Dầu" },
            { code: "711", name: "Huyện Bến Cầu" },
            { code: "712", name: "Huyện Trảng Bàng" }
        ],
        // Bình Dương
        "74": [
            { code: "718", name: "Thành phố Thủ Dầu Một" },
            { code: "719", name: "Huyện Bàu Bàng" },
            { code: "720", name: "Huyện Dầu Tiếng" },
            { code: "721", name: "Thị xã Bến Cát" },
            { code: "722", name: "Huyện Phú Giáo" },
            { code: "723", name: "Thành phố Tân Uyên" },
            { code: "724", name: "Thành phố Dĩ An" },
            { code: "725", name: "Thành phố Thuận An" },
            { code: "726", name: "Huyện Bắc Tân Uyên" }
        ],
        // Đồng Nai
        "75": [
            { code: "731", name: "Thành phố Biên Hòa" },
            { code: "732", name: "Thành phố Long Khánh" },
            { code: "734", name: "Huyện Tân Phú" },
            { code: "735", name: "Huyện Vĩnh Cửu" },
            { code: "736", name: "Huyện Định Quán" },
            { code: "737", name: "Huyện Trảng Bom" },
            { code: "738", name: "Huyện Thống Nhất" },
            { code: "739", name: "Huyện Cẩm Mỹ" },
            { code: "740", name: "Huyện Long Thành" },
            { code: "741", name: "Huyện Xuân Lộc" },
            { code: "742", name: "Huyện Nhơn Trạch" }
        ],
        // Bà Rịa - Vũng Tàu
        "77": [
            { code: "747", name: "Thành phố Vũng Tàu" },
            { code: "748", name: "Thành phố Bà Rịa" },
            { code: "750", name: "Huyện Châu Đức" },
            { code: "751", name: "Huyện Xuyên Mộc" },
            { code: "752", name: "Huyện Long Điền" },
            { code: "753", name: "Huyện Đất Đỏ" },
            { code: "754", name: "Thị xã Phú Mỹ" },
            { code: "755", name: "Huyện Côn Đảo" }
        ],
        // Hồ Chí Minh
        "79": [
        ]
    };
    
    // Thêm các tỉnh vào dropdown
    provinces.forEach(province => {
        const option = document.createElement('option');
        option.value = province.code;
        option.textContent = province.name;
        provinceSelect.appendChild(option);
    });
    
    // Xử lý sự kiện khi chọn tỉnh/thành phố
    provinceSelect.addEventListener('change', function() {
        const provinceCode = this.value;
        
        // Reset và disable dropdown quận/huyện
        districtSelect.innerHTML = '<option value="">Quận/Huyện</option>';
        
        if (!provinceCode) {
            districtSelect.disabled = true;
            return;
        }
        
        // Enable dropdown quận/huyện
        districtSelect.disabled = false;
        
        // Tải danh sách quận/huyện theo tỉnh đã chọn
        if (districtsByProvince[provinceCode]) {
            districtsByProvince[provinceCode].forEach(district => {
                const option = document.createElement('option');
                option.value = district.code;
                option.textContent = district.name;
                districtSelect.appendChild(option);
            });
        } else {
            console.log('Không có dữ liệu quận/huyện cho tỉnh/thành phố này');
        }
    });
});