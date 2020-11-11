using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO
{
    public class HoaDonDAO
    {

        QuanLy_CafeEntities hdEntity = new QuanLy_CafeEntities();

        public int laySoHD()
        {
            var slhd = from hd in hdEntity.hoadons select hd;
            return slhd.Count() + 1;
        }
        public List<SanPhamDTO> LayDSSP_HD(string maHD)
        {
            List<SanPhamDTO> dssp = new List<SanPhamDTO>();
            foreach (ct_hoadon sp in hdEntity.ct_hoadon.Where(c => c.mahd == maHD).ToList())
            {
                SanPhamDTO spAdd = new SanPhamDTO();
                spAdd.masp = sp.masp;
                spAdd.tensp = sp.sanpham.tensp;
                spAdd.slmua = sp.sl_mua.Value;
                spAdd.giaBan = sp.Giaban.Value;
                dssp.Add(spAdd);
            }
            return dssp;
        }

    }
}
