using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;

namespace DAO
{
    public class ClassDAOEntity
    {
        QuanLy_CafeEntities QlCFEntity = new QuanLy_CafeEntities();

        //Ban
        public List<BanDTO> LayDSBan(int kind)
        {
            List<BanDTO> dsBan = new List<BanDTO>();
            if(kind==0)
            {
                dsBan = QlCFEntity.bans.Where(u => u.xoaBan > 0).Select(u => new BanDTO
                {
                    masoban = u.masoban,
                    socho = u.socho.Value,
                    maghep = u.masoGhep_Tach.Value,
                    TrangThai = u.xoaBan.Value
                }).ToList();
            }
            else
            {
                dsBan = QlCFEntity.bans.Where(u => u.xoaBan == 1).Select(u => new BanDTO
                {
                    masoban = u.masoban,
                    socho = u.socho.Value,
                    maghep = u.masoGhep_Tach.Value,
                    TrangThai = u.xoaBan.Value
                }).ToList();
            }
            return dsBan;
        }

        //Nhân viên

               

        public List<ChucVuDTO> LayDSCV_NV()
        {
            List<ChucVuDTO> dscv = new List<ChucVuDTO>();

            dscv = QlCFEntity.LoaiNVs.Where(c => c.xoaLNV == 1).Select(c => new ChucVuDTO
            {
                maCV=c.maloainv,
                tenCV=c.Tenchucvu,
                xoaCV=c.xoaLNV.Value
            }).ToList();

            return dscv;
        }

        //Sửa tất cả hàm Lấy nhân viên 
        public NhanVienDTO LayNV(string matk)
        {            
            NhanVienDTO nv = new NhanVienDTO();
            try
            {
                if(matk=="tk_01")
                {
                    nv.maNV = "Admin";
                    nv.tenNV = "Min";
                }
                else
                {
                    nv = QlCFEntity.NhanViens.Where(u => u.xoaNV == 1 && u.taikhoan == matk)
                    .Select(u => new NhanVienDTO
                    {
                        maNV = u.manv,
                        hoNV = u.Honv,
                        tenNV = u.Tennv,
                        ngaySinh = u.NgaySinh.Value,
                        GioiTinh = u.Nu.Value == true ? "Nữ" : "Nam",
                        diaChi = u.diachi,
                        sdt = u.sdt,
                        maTK = u.taikhoan,
                        MaCV = u.LoaiNV.Tenchucvu,
                        ngayBD = u.ngay_BD.Value,
                        soNgaynghi = u.songaynghi.Value,
                        XoaNV = u.xoaNV.Value,
                    }).SingleOrDefault();

                    //Lấy lương
                    LoaiNV lnv = new LoaiNV();
                    lnv = QlCFEntity.LoaiNVs.Where(n => n.Tenchucvu == nv.MaCV).SingleOrDefault();
                    nv.Luong = lnv.luongcoban.Value;

                    //Tài khoản đang dùng
                    AccountDAO acDAO = new AccountDAO();
                    nv.matk = new AccountDTO();
                    nv.matk = acDAO.LayTK(nv.maTK);
                }
                
            }
            catch
            {

            }
            
            
            return nv;
        }

        public NhanVienDTO LayNV_ma(string manv)
        {
            NhanVienDTO nv = new NhanVienDTO();
            nv = QlCFEntity.NhanViens.Where(n => n.manv == manv && n.xoaNV > 0).Select(n => new NhanVienDTO
            {
                maNV=n.manv,
                hoNV=n.Honv,
                tenNV=n.Tennv,
                ngaySinh=n.NgaySinh.Value,
                GioiTinh=n.Nu.Value == true ? "Nữ" : "Nam",
                diaChi = n.diachi,
                sdt = n.sdt,
                MaCV = n.LoaiNV.Tenchucvu,
                ngayBD = n.ngay_BD.Value,
                soNgaynghi = n.songaynghi.Value,
                maTK=n.taikhoan,
                XoaNV = n.xoaNV.Value,
                Luong =  n.LoaiNV.luongcoban.Value,
            }).SingleOrDefault();

            return nv;
        }

        public List<NhanVienDTO> layDSNV(string matk)
        {
            List<NhanVienDTO> dsnv = new List<NhanVienDTO>();
            dsnv = QlCFEntity.NhanViens.Where(u => u.xoaNV == 1 && u.taikhoan == matk).Select(u => new NhanVienDTO
            {
                maNV = u.manv,
                hoNV = u.Honv,
                tenNV = u.Tennv,
                ngaySinh = u.NgaySinh.Value,
                ngayBD=u.ngay_BD.Value,
                GioiTinh = u.Nu.Value == true ? "Nữ" : "Nam",
                Luong=u.LoaiNV.luongcoban.Value,
                sdt = u.sdt,
                diaChi = u.diachi,
                MaCV = u.LoaiNV.Tenchucvu,
                maTK=u.taikhoan,
            }).ToList();
            foreach(NhanVienDTO nv in dsnv)
            {
                nv.matk = QlCFEntity.Taikhoans.Where(t => t.matk == nv.maTK && t.xoaTK > 0).Select(t => new AccountDTO
                {
                    maTK=t.matk,
                    userName=t.Ten_login,
                    pass=t.pass,
                    TrangThai=t.xoaTK.Value,
                }).SingleOrDefault();
            }
            return dsnv;
        }

        public List<NhanVienDTO> layDSNV()
        {
            List<NhanVienDTO> dsnv = new List<NhanVienDTO>();
            dsnv = QlCFEntity.NhanViens.Where(u => u.xoaNV > 0).Select(u => new NhanVienDTO
            {
                maNV = u.manv,
                hoNV = u.Honv == null ? "" : u.Honv,
                tenNV = u.Tennv == null ? "" : u.Tennv,
                ngaySinh = u.NgaySinh.Value,
                ngayBD=u.ngay_BD.Value,
                maTK=u.taikhoan,
                MaCV = u.LoaiNV.Tenchucvu,
                Luong =u.LoaiNV.luongcoban.Value,
                GioiTinh = u.Nu.Value == true ? "Nữ" : "Nam",
                sdt = u.sdt,
                diaChi = u.diachi,
                XoaNV=u.xoaNV.Value,                
            }).ToList();
            foreach (NhanVienDTO nv in dsnv)
            {
                nv.matk = QlCFEntity.Taikhoans.Where(t => t.matk == nv.maTK && t.xoaTK == 1).Select(t => new AccountDTO
                {
                    maTK = t.matk,
                    userName = t.Ten_login,
                    pass = t.pass,
                    TrangThai = t.xoaTK.Value,
                }).SingleOrDefault();
            }


            return dsnv;
        }
        
        public List<NhanVienDTO> DSNVPQ(string maquyen)
        {
            AccountDAO acDAO = new AccountDAO();
            List<NhanVienDTO> lstNV = new List<NhanVienDTO>();
            try
            {                
                string qug = acDAO.convert_TenQuyen(maquyen);
                lstNV = QlCFEntity.NhanViens.Where(n => n.xoaNV == 2 && n.chucvu == qug)
                    .Select(v => new NhanVienDTO
                    {
                        maNV = v.manv,
                        MaCV = v.chucvu,
                    }).ToList();

                return lstNV;
            }
            catch
            {
                return lstNV;
            }
            
        }

        public string TaoMaNhanVien()
        {
            int slnv = QlCFEntity.NhanViens.Count();

            if (slnv < 10)
                return "nv_0" + slnv;

            return "nv_" + slnv;
        }

        public bool ThemNV(NhanVienDTO nv)
        {
            AccountDAO a = new AccountDAO();
            try
            {
                NhanVien nvmoi = new NhanVien();
                nvmoi.manv = TaoMaNhanVien();
                nvmoi.Honv = nv.hoNV;
                nvmoi.Tennv = nv.tenNV;
                nvmoi.NgaySinh = nv.ngaySinh;
                nvmoi.taikhoan = "tk_02";
                nvmoi.diachi = nv.diaChi;
                nvmoi.chucvu = a.convert_TenQuyen(nv.MaCV);
                nvmoi.ngay_BD = DateTime.Now;
                nvmoi.Nu = nv.GioiTinh == "Nam" ? false : true;
                nvmoi.sdt = nv.sdt;
                nvmoi.songaynghi = nv.soNgaynghi;
                nvmoi.xoaNV = 2;

                QlCFEntity.NhanViens.Add(nvmoi);
                QlCFEntity.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        //Sửa DelNV
        public bool DelNV(string manv)
        {
            try
            {
                foreach (hoadon hd in QlCFEntity.hoadons.Where(h=>h.nv_lapHD==manv).ToList())
                {
                    if(hd.xoaHD>0)
                    {
                        return false;
                    }
                }
                NhanVien nv = QlCFEntity.NhanViens.Where(n => n.manv == manv).SingleOrDefault();
                nv.xoaNV = 0;
                QlCFEntity.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool CapQuyenDangNhap(string matk, string manv)
        {
            try
            {                
                NhanVien nv = QlCFEntity.NhanViens.Where(n => n.manv == manv && n.xoaNV > 0).SingleOrDefault();
                nv.taikhoan = matk;
                nv.xoaNV = 1;
                QlCFEntity.SaveChanges();
                
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool ThuHoiTK(string matk, string manv)
        {
            try
            {
                NhanVien nv = QlCFEntity.NhanViens.Where(n => n.manv == manv && n.xoaNV > 0).SingleOrDefault();
                nv.xoaNV = 2;
                QlCFEntity.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateNV(NhanVienDTO nv)
        {
            AccountDAO a = new AccountDAO();
            try
            {
                NhanVien nvmoi = QlCFEntity.NhanViens.Where(n => n.manv == nv.maNV).SingleOrDefault();
                if (nvmoi == null)
                    return false;
                nvmoi.Honv = nv.hoNV;
                nvmoi.Tennv = nv.tenNV;
                nvmoi.NgaySinh = nv.ngaySinh;
                nvmoi.diachi = nv.diaChi;
                nvmoi.chucvu = a.convert_TenQuyen(nv.MaCV);
                nvmoi.ngay_BD = DateTime.Now;
                nvmoi.Nu = nv.GioiTinh == "Nam" ? false : true;
                nvmoi.sdt = nv.sdt;
                nvmoi.songaynghi = nv.soNgaynghi;
                nvmoi.xoaNV =Convert.ToByte(nv.XoaNV);

                QlCFEntity.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        //Khách hàng
        public string CreateMaKh(int lkh)
        {
            if(lkh==0)
            {
                int tongkh = QlCFEntity.khachhangs.Count();
                int khV = QlCFEntity.khachhangs.Where(u => u.makh.StartsWith("KHV")).Count();
                int kh = tongkh - khV + 1;
                if (kh < 10)
                    return "kh000" + kh;
                if (kh < 100)
                    return "kh00" + kh;
                if (kh < 1000)
                    return "kh0" + (kh);
                return "kh" + kh;
            }
            else
            {
                int khV = QlCFEntity.khachhangs.Where(u => u.makh.StartsWith("KHV")).Count() + 1;
                if (khV < 10)
                    return "KHV00" + khV;
                if (khV < 100)
                    return "KHV0" + khV;
                else
                    return "KHV" + khV;
            }
            
        }

        public void themKH(string makh)
        {
            khachhang kh = new khachhang();
            kh.makh = makh;
            kh.Tenkh = "";
            kh.sdt = "";
            kh.point = 0;
            kh.xoaKH = 1;
            QlCFEntity.khachhangs.Add(kh);
            QlCFEntity.SaveChanges();            
        }

        public List<KhachHangDTO> layDSKH()
        {
            List<KhachHangDTO> Dskh = new List<KhachHangDTO>();
            Dskh = QlCFEntity.khachhangs.Where(u => u.xoaKH == 1 && u.makh.StartsWith("KHV")).Select(u => new KhachHangDTO
            {
                maKH = u.makh,
                tenKH = u.Tenkh,
                sdt = u.sdt,
                point = u.point.Value,
                xoaKH = u.xoaKH.Value,
            }).ToList();
            return Dskh;
        }

        public KhachHangDTO layKH(string makh)
        {
            KhachHangDTO kh = new KhachHangDTO();
            kh = QlCFEntity.khachhangs.Where(u => u.xoaKH == 1 && u.makh.StartsWith("KHV")).Select(u => new KhachHangDTO
            {
                maKH = u.makh,
                tenKH = u.Tenkh,
                sdt = u.sdt,
                point = u.point.Value,
                xoaKH = u.xoaKH.Value,
            }).SingleOrDefault();
            return kh;
        }

        public bool DKKH(KhachHangDTO kh)
        {
            khachhang customer = new khachhang();           

            try
            {
                customer.makh = CreateMaKh(1);
                customer.Tenkh = kh.tenKH;
                customer.sdt = kh.sdt;
                customer.point = 0;
                customer.xoaKH = 1;
                QlCFEntity.khachhangs.Add(customer);
                QlCFEntity.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateKH(KhachHangDTO kh)
        {
            try
            {
                khachhang customer = QlCFEntity.khachhangs.Where(u => u.makh == kh.maKH).SingleOrDefault();
                customer.Tenkh = kh.tenKH;
                customer.sdt = kh.sdt;
                customer.point = kh.point;
                QlCFEntity.SaveChanges();
                return true;
            }
            catch 
            {
                return false;
            }
        }

        public int DelKH(string makh)
        {
            try
            {
                khachhang customer = QlCFEntity.khachhangs.Where(u => u.makh == makh).SingleOrDefault();
                foreach(hoadon hd in QlCFEntity.hoadons.Where(h=>h.makh==makh).ToList())
                {
                    if (hd.xoaHD == 1)
                        return 2;
                }
                customer.xoaKH = 0;
                QlCFEntity.SaveChanges();
                return 1;
            }
            catch
            {
                return 0;
            }
        }

        public string Tennsx(string mancc)
        {
            try
            {
                NhaCungCap ncc = QlCFEntity.NhaCungCaps.Where(n => n.mancc == mancc).SingleOrDefault();
                return ncc.Tenncc;
            }
            catch
            {
                return "";
            }
            
        }

        //Hóa đơn
        public HoaDonDTO LayTTHD(string maHD)
        {
            HoaDonDTO hd = new HoaDonDTO();
            hd = QlCFEntity.hoadons.Where(u => u.mahd == maHD).Select(u => new HoaDonDTO
            {
                maHD = u.mahd,
                khachHang = new KhachHangDTO
                {
                    maKH = u.khachhang.makh,
                    tenKH = u.khachhang.Tenkh,
                    sdt = u.khachhang.sdt,
                    point = u.khachhang.point.Value,
                    xoaKH = u.khachhang.xoaKH.Value
                },
                ngayLap = u.ngaylap.Value,
                tongThanhToan = u.tong_TT.Value,
                soban = new BanDTO
                {
                    masoban = u.ban.masoban,
                    socho = u.ban.socho.Value,
                    maghep = u.ban.masoGhep_Tach.Value,
                    TrangThai = u.ban.xoaBan.Value
                },
                nv_LapHD = new NhanVienDTO
                {
                    maNV = u.NhanVien.manv,
                },
                xoaHD = u.xoaHD.Value
            }).SingleOrDefault();
            hd.dssp = new List<SanPhamDTO>();
            foreach (ct_hoadon sp in QlCFEntity.ct_hoadon.Where(c => c.mahd == maHD).ToList())
            {
                SanPhamDTO spAdd = new SanPhamDTO();
                spAdd.masp = sp.masp;
                spAdd.tensp = sp.sanpham.tensp;
                spAdd.dongia = sp.sanpham.don_gia.Value;
                spAdd.slmua = sp.sl_mua.Value;
                spAdd.giaBan = sp.Giaban.Value;
                hd.dssp.Add(spAdd);
            }
            return hd;
        }

        public HoaDonDTO LayTTHD(DateTime Ngay)
        {
            HoaDonDTO hd = new HoaDonDTO();

            hd = QlCFEntity.hoadons.Where(u => u.ngaylap.Value.Year == Ngay.Year
                 && u.ngaylap.Value.Month == Ngay.Month && u.xoaHD == 1).Select(u => new HoaDonDTO
                 {
                    maHD = u.mahd,
                    maKH = u.makh,
                    khachHang = new KhachHangDTO
                    {
                        maKH = u.khachhang.makh,
                        tenKH = u.khachhang.Tenkh,
                        sdt = u.khachhang.sdt,
                        point = u.khachhang.point.Value,
                        xoaKH = u.khachhang.xoaKH.Value
                    },
                    ngayLap = u.ngaylap.Value,
                    tongThanhToan = u.tong_TT.Value,
                    soban = new BanDTO
                    {
                        masoban = u.ban.masoban,
                        socho = u.ban.socho.Value,
                        maghep = u.ban.masoGhep_Tach.Value,
                        TrangThai = u.ban.xoaBan.Value
                    },
                    nv_LapHD = new NhanVienDTO
                    {
                        maNV = u.NhanVien.manv,

                    },
                    xoaHD = u.xoaHD.Value
                 }).SingleOrDefault();
            hd.dssp = new List<SanPhamDTO>(LayTTHD(hd.maHD).dssp);

            return hd;
        }

        public HoaDonDTO LayTTHDBan(string maban)
        {
            HoaDonDTO hd = new HoaDonDTO();
            hd = QlCFEntity.hoadons.Where(u => u.so_ban == maban && u.ngaylap.Value.Year == DateTime.Now.Year
                 && u.ngaylap.Value.Month == DateTime.Now.Month && u.ngaylap.Value.Day == DateTime.Now.Day)
                 .Select(u => new HoaDonDTO
                {
                    maHD = u.mahd,
                    khachHang = new KhachHangDTO
                    {
                        maKH = u.khachhang.makh,
                        tenKH = u.khachhang.Tenkh,
                        sdt = u.khachhang.sdt,
                        point = u.khachhang.point.Value,
                        xoaKH = u.khachhang.xoaKH.Value
                    },
                    ngayLap = u.ngaylap.Value,
                    tongThanhToan = u.tong_TT.Value,
                    soban = new BanDTO
                    {
                        masoban = u.ban.masoban,
                        socho = u.ban.socho.Value,
                        maghep = u.ban.masoGhep_Tach.Value,
                        TrangThai = u.ban.xoaBan.Value
                    },
                    nv_LapHD = new NhanVienDTO
                    {
                        maNV = u.NhanVien.manv,
                    },
                    xoaHD = u.xoaHD.Value
                }).SingleOrDefault();
            hd.dssp = new List<SanPhamDTO>();
            foreach (ct_hoadon sp in QlCFEntity.ct_hoadon.Where(c => c.mahd == hd.maHD).ToList())
            {
                SanPhamDTO spAdd = new SanPhamDTO();
                spAdd.masp = sp.masp;
                spAdd.tensp = sp.sanpham.tensp;
                spAdd.slmua = sp.sl_mua.Value;
                spAdd.giaBan = sp.Giaban.Value;
                hd.dssp.Add(spAdd);
            }
            return hd;
        }

        public List<HoaDonDTO> Convert_DSHD(List<hoadon> dshd)
        {
            List<HoaDonDTO> dshdDTO = new List<HoaDonDTO>();
            foreach (hoadon hd in dshd)
            {
                HoaDonDTO hdDTO = new HoaDonDTO();
                hdDTO.maHD = hd.mahd;
                hdDTO.maKH = hd.makh;
                hdDTO.maNV = hd.nv_lapHD;
                hdDTO.maBan = hd.so_ban;

                hdDTO.khachHang = QlCFEntity.khachhangs.Where(u => u.makh == hd.makh).Select(u => new KhachHangDTO
                {
                    maKH=u.makh,
                    tenKH=u.Tenkh,
                    sdt=u.sdt,
                    point=u.point.Value,
                    xoaKH=u.xoaKH.Value
                }).SingleOrDefault();                                     

                hdDTO.ngayLap = hd.ngaylap.Value;
                hdDTO.tongThanhToan = hd.tong_TT.Value;

                hdDTO.soban = QlCFEntity.bans.Where(b => b.masoban == hd.so_ban).Select(b => new BanDTO
                {
                    masoban=b.masoban,
                    socho=b.socho.Value,
                    maghep=b.masoGhep_Tach.Value,
                    TrangThai=b.xoaBan.Value
                }).SingleOrDefault();
                

                hdDTO.nv_LapHD = QlCFEntity.NhanViens.Where(v => v.manv == hd.nv_lapHD).Select(v => new NhanVienDTO
                {
                    maNV = v.manv,
                    hoNV = v.Honv,
                    tenNV = v.Tennv,
                    XoaNV = hd.xoaHD.Value,
                }).SingleOrDefault();

                dshdDTO.Add(hdDTO);
            }
            return dshdDTO;
        }

        public List<HoaDonDTO> LayDSHD(DateTime Ngay,int loai)
        {
            List<HoaDonDTO> dshd = new List<HoaDonDTO>();
            switch(loai)
            {
                case 1:
                    {
                        dshd = Convert_DSHD(QlCFEntity.hoadons.Where(u => u.ngaylap.Value.Year == Ngay.Year
                               && u.xoaHD == 1).ToList());
                        foreach (HoaDonDTO hd in dshd)
                        {
                            hd.dssp = LayTTHD(hd.maHD).dssp;
                        }
                    }
                    break;
                case 2:
                    {
                        dshd = Convert_DSHD(QlCFEntity.hoadons.Where(u => u.ngaylap.Value.Year == Ngay.Year
                        &&u.ngaylap.Value.Month== Ngay.Month && u.xoaHD == 1).ToList());
                        foreach (HoaDonDTO hd in dshd)
                        {
                            hd.dssp = LayTTHD(hd.maHD).dssp;
                        }
                    }
                    break;
                default:
                    {
                        dshd = Convert_DSHD(QlCFEntity.hoadons.Where(u => u.ngaylap.Value.Year == Ngay.Year
                        &&u.ngaylap.Value.Month== Ngay.Month && u.ngaylap.Value.Day == Ngay.Day
                        && u.xoaHD == 1).ToList());
                        foreach (HoaDonDTO hd in dshd)
                        {
                            hd.dssp = LayTTHD(hd.maHD).dssp;
                        }
                    }
                    break;
            }
            
            return dshd;
        }

        //Sửa ThemHD
        public bool ThemHD(HoaDonDTO hd)
        {
            try
            {
                hoadon hdNew = new hoadon();
                hdNew.mahd = hd.maHD;
                if (hd.khachHang == null)
                {
                    hdNew.makh = CreateMaKh(0);
                    themKH(hdNew.makh);
                }
                else { hdNew.makh = hd.khachHang.maKH; }
                hdNew.ngaylap = hd.ngayLap;
                hdNew.nv_lapHD = hd.nv_LapHD.maNV;
                hdNew.so_ban = hd.soban.masoban;
                hdNew.tong_TT = hd.tongThanhToan;
                hdNew.xoaHD = 2;
                QlCFEntity.hoadons.Add(hdNew);
                ThemCTHD(hd);
                QlCFEntity.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
            
            
        }

        public void ThemCTHD(HoaDonDTO hd)
        {
            foreach (SanPhamDTO sp in hd.dssp)
            {
                ct_hoadon cthd = new ct_hoadon();
                cthd.mahd = hd.maHD;
                cthd.masp = sp.masp;
                cthd.sl_mua = sp.slmua;
                cthd.Giaban = sp.giaBan;
                cthd.sales =((sp.slmua*sp.giaBan*sp.Sales)/100);
                cthd.xoaCTHD = 1;
                QlCFEntity.ct_hoadon.Add(cthd);
            }
            QlCFEntity.SaveChanges();
        }

        public bool UpdateHD(HoaDonDTO hd)
        {
            try
            {
                hoadon hdEdit = QlCFEntity.hoadons.Where(u => u.mahd == hd.maHD).SingleOrDefault();
                hdEdit.nv_lapHD = hd.maNV;
                hdEdit.so_ban = hd.soban.masoban;
                hdEdit.tong_TT = hd.tongThanhToan;
                UpdateCTHD(hd);
                QlCFEntity.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }            
        }

        public void UpdateCTHD(HoaDonDTO hd)
        {
            List<ct_hoadon> cthd = QlCFEntity.ct_hoadon.Where(u => u.mahd == hd.maHD && u.xoaCTHD == 1).ToList();
            foreach(ct_hoadon ct in cthd)
            {
                QlCFEntity.ct_hoadon.Remove(ct);
            }            
            QlCFEntity.SaveChanges();
            ThemCTHD(hd);
        }

        //Sửa Xóa hóa đơn
        public bool XoaHD(String mahd)
        {
            try
            {
                hoadon hdXoa = QlCFEntity.hoadons.Where(u => u.mahd == mahd).SingleOrDefault();
                hdXoa.xoaHD = 0;

                ban b = QlCFEntity.bans.Where(tb => tb.masoban == hdXoa.so_ban).SingleOrDefault();
                b.xoaBan = 1;

                List<ct_hoadon> cthdXoa = QlCFEntity.ct_hoadon.Where(u => u.mahd == mahd).ToList();
                foreach(ct_hoadon ct in cthdXoa)
                {
                    ct.xoaCTHD = 0;
                }
                QlCFEntity.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }            
        }

        
    }
}
