using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;

namespace DAO
{
    public class SanPhamDAO
    {
        QuanLy_CafeEntities qlcfEntity = new QuanLy_CafeEntities();

        public List<SanPhamDTO> Load_SP(string masp)
        {

            List<SanPhamDTO> sp = qlcfEntity.sanphams.Where(u => u.masp.StartsWith(masp)).Select(u => new SanPhamDTO
            {
                masp = u.masp,
                tensp = u.tensp,
                dongia = u.don_gia.Value,
                giaBan = u.don_gia.Value,
                Sales = 0,
                slmua = 0,
                XoaSP = u.xoaSP.Value,
                NCC = u.nhacungcap,
            }).ToList();
            foreach (SanPhamDTO s in sp)
            { s.loaisp = ConVert_ma_loaiThucUong(s.masp); }

            return sp;
        }

        public SanPhamDTO LaySP_Masp(string masp)
        {
            SanPhamDTO sp = qlcfEntity.sanphams.Where(u => u.masp == masp).Select(u => new SanPhamDTO
            {
                masp = u.masp,
                tensp = u.tensp,
                dongia = u.don_gia.Value,
                giaBan = u.don_gia.Value,
                Sales = 0,
                slmua = 0,
                XoaSP = u.xoaSP.Value,
                NCC = u.nhacungcap,
            }).SingleOrDefault();
            sp.loaisp = ConVert_ma_loaiThucUong(masp);

            return sp;
        }

        public List<SanPhamDTO> Load_DSSP_Loai(string masp)
        {
            List<SanPhamDTO> dssp = new List<SanPhamDTO>();
            foreach (sanpham sp in qlcfEntity.sanphams.Where(u=> u.xoaSP ==1 && u.masp.StartsWith(masp)))
            {
                SanPhamDTO spAdd = new SanPhamDTO();
                spAdd.masp = sp.masp;
                spAdd.tensp = sp.tensp;
                spAdd.dongia = sp.don_gia.Value;
                spAdd.giaBan = sp.don_gia.Value;
                dssp.Add(spAdd);
            }
            return dssp;
        }

        public List<SanPhamDTO> LayDSThucUong()
        {
            List<SanPhamDTO> dsTU = new List<SanPhamDTO>();

            dsTU = qlcfEntity.sanphams.Where(t => t.masp.StartsWith("food") == false && t.xoaSP == 1)
                .Select(t => new SanPhamDTO
            {
                masp=t.masp,
                tensp=t.tensp,
                dongia=t.don_gia.Value,
                giaBan=t.don_gia.Value,
                Sales=0,
                slmua=0,
                XoaSP=1,
                NCC=t.NhaCungCap1.mancc,
            }).ToList();

            foreach (SanPhamDTO sp in dsTU)
            {
                sp.loaisp = ConVert_ma_loaiThucUong(sp.masp);
            }
                return dsTU;
        }

        public List<SanPhamDTO> LayDSThucAn()
        {
            List<SanPhamDTO> dsTA = new List<SanPhamDTO>();

            dsTA = qlcfEntity.sanphams.Where(t => t.masp.StartsWith("food") == true && t.xoaSP == 1)
                .Select(t => new SanPhamDTO
            {
                masp = t.masp,
                tensp = t.tensp,
                dongia = t.don_gia.Value,
                giaBan = t.don_gia.Value,
                Sales = 0,
                slmua = 0,
                XoaSP = 1,
                NCC = t.NhaCungCap1.mancc,
            }).ToList();

            return dsTA;
        }

        public List<NhaCungCapDTO> LayDSNCC()
        {
            List<NhaCungCapDTO> dsncc = new List<NhaCungCapDTO>();

            dsncc = qlcfEntity.NhaCungCaps.Where(nc => nc.Xoancc == 1).Select(nc => new NhaCungCapDTO
            {
                mancc=nc.mancc,
                tenncc=nc.Tenncc,
                xoancc=nc.Xoancc.Value,
            }).ToList();

           return dsncc;
        }

        

        public SanPhamDTO laySP_MaSP(string maSP)
        {
            SanPhamDTO sp = new SanPhamDTO();
            sp = qlcfEntity.sanphams.Where(u => u.masp == maSP).Select(u => new SanPhamDTO
            {
                masp = u.masp,
                tensp = u.tensp,
                dongia = u.don_gia.Value,
                giaBan = u.don_gia.Value,
                NCC=u.nhacungcap,
                XoaSP = u.xoaSP.Value,               
            }).SingleOrDefault();
            sp.loaisp = ConVert_ma_loaiThucUong(sp.masp);
            return sp;
        }
        
        public string ConVert_ma_loaiThucUong(string maTU)
        {
            string ten = maTU.StartsWith("ts") ? "Trà sữa" : maTU.StartsWith("tea") ? "Tea" :
                   maTU.StartsWith("ctl") ? "CookTail" : maTU.StartsWith("cf") ? "Cà phê" :
                   maTU.StartsWith("nn") ? "Nước ngọt" : maTU.StartsWith("st") ? "Sinh tố" : "Thức ăn nhanh";
            return ten;
        }

        public SanPhamDTO LaySP(string masp)
        {
            SanPhamDTO sanPhamDTO = new SanPhamDTO();
            sanPhamDTO = qlcfEntity.sanphams.Where(u => u.masp == masp).Select(u => new SanPhamDTO
            {
                masp = u.masp,
                tensp = u.tensp,
                dongia = u.don_gia.Value,
                NCC = u.nhacungcap,
                giaBan=u.don_gia.Value,
                Sales=0,
                slmua=0,
                XoaSP = u.xoaSP.Value
            }).SingleOrDefault();
            return sanPhamDTO;
        }
    }
}
