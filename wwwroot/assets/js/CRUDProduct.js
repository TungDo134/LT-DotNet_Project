var j = jQuery.noConflict();
j("#submitBtn").on("click", function () {

    // lay data ve
    const product = {
        TenSp: j("#name").val(),
        MaDanhMuc: j("#select").val(),
        DonGia: parseFloat(j("#price").val().trim().replace(/[^\d.-]/g, '')),
        KhuyenMai: null,
        ThongTinSp: j("#des").val(),
        KhoiLuong: j("#weight").val(),
        HinhAnh: j("#img").val().split("\\").pop()
    };

  

    console.log(product);

    j.ajax({
        url: "https://localhost:7258/api/ProductAPI/add", 
        type: "POST", 
        contentType: "application/json", // Dữ liệu gửi đi ở định dạng JSON
        data: JSON.stringify(product), // Chuyển đổi đối tượng product thành JSON
        success: function (response) {
            // Xử lý khi yêu cầu thành công
            j("#msg").text(response.message) // Hiển thị thông báo từ API
            //alert(response.message); 
            
        },
        error: function (xhr, status, error) {
            // Xử lý khi có lỗi
            console.error("Lỗi:", xhr.responseText);
            alert("Có lỗi xảy ra. Vui lòng thử lại.");
        }
    });
})


// Cập nhật sản phẩm
j("#BtnUpdate").on("click", function () {


    let img;
    if (j("#img").val().trim() !== "") { // Kiểm tra giá trị không rỗng
        img = j("#img").val().split("\\").pop(); // Lấy giá trị từ #img
    } else {
        img = j("#oldImg").val(); // Lấy giá trị từ #oldImg
    }

    console.log("Giá trị của img:", img);



    // lay data ve
    const product = {
        MaSp: j("#id").val(),
        TenSp: j("#name").val(),
        MaDanhMuc: j("#select").val(),
        DonGia: parseFloat(j("#price").val().trim().replace(/[^\d.-]/g, '')),
        KhuyenMai: null,
        ThongTinSp: j("#des").val(),
        KhoiLuong: j("#weight").val(),
        HinhAnh: img
    };

    console.log(product);

    j.ajax({
        url: "https://localhost:7258/api/ProductAPI/edit",
        type: "PUT",
        contentType: "application/json", // Dữ liệu gửi đi ở định dạng JSON
        data: JSON.stringify(product), // Chuyển đổi đối tượng product thành JSON
        success: function (response) {
            // Xử lý khi yêu cầu thành công
            j("#msg").text(response.message) // Hiển thị thông báo từ API
            j("#btnBack").text("Quay về trang sản phẩm");
            j("#btnBack").css("display", "inline-block");

        },
        error: function (xhr, status, error) {
            // Xử lý khi có lỗi
            console.error("Lỗi:", xhr.responseText);
            alert("Có lỗi xảy ra. Vui lòng thử lại.");
        }
    });
})


// xoa sp
function deleteProduct(id) {
    const url = 'https://localhost:7258/api/ProductAPI/delById/' + id;

    if (!confirm("Bạn có chắc chắn muốn thực hiện hành động này?")) {
        event.preventDefault(); // Hủy bỏ hành động mặc định
    } else {

        j.ajax({
            url: url,
            type: "DELETE",
            success: function (response) {
                alert(response.message); // Thông báo kết quả
                j(`tr[data-id="${id}"]`).remove();

            },
            error: function (xhr) {
                console.error("Error:", xhr.responseText);
                alert("Có lỗi xảy ra trong quá trình xóa sản phẩm.");
            }
        });
    }
}