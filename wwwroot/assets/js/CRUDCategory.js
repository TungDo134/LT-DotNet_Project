
var j = jQuery.noConflict();

// thme danh muc
j("#submitBtn").on("click", function () {

    // lay data ve
    const category = {
        MaDanhMuc: j("#cateID").val(),
        TenDanhMuc: j("#name").val(),
        HinhDanhMuc: j("#cateImg").val().split("\\").pop()
    };
    console.log(category);

    // thuc hien them danh muc
    j.ajax({
        url: "https://localhost:7258/api/CateAPI/add",
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify(category),

        success: function (response) {
            alert(response.message);
            location.reload();

        },
        error: function (xhr, status, error) {
            console.error("Lỗi:", xhr.responseText);
            alert("Có lỗi xảy ra. Vui lòng thử lại.");
        }
    });
})

// chinh sua danh muc
j("#BtnUpdate").on("click", function () {

    let img;
    if (j("#cateImg").val().trim() !== "") { // Kiểm tra giá trị không rỗng
        img = j("#cateImg").val().split("\\").pop(); // Lấy giá trị từ #cateImg
    } else {
        img = j("#oldImg").val(); // Lấy giá trị từ #oldImg
    }

    // lay data ve
    const category = {
        MaDanhMuc: j("#cateID").val(),
        TenDanhMuc: j("#name").val(),
        HinhDanhMuc: img
    };

    console.log(category);
    // thuc hien them danh muc
    j.ajax({
        url: "https://localhost:7258/api/CateAPI/update",
        type: "PUT",
        contentType: "application/json",
        data: JSON.stringify(category),

        success: function (response) {
            alert(response.message);
            j("#btnBack").text("Quay về trang danh mục");
            j("#button").css("display","block");
          
        },
        error: function (xhr, status, error) {
            console.error("Lỗi:", xhr.responseText);
            alert("Có lỗi xảy ra. Vui lòng thử lại.");
        }
    });
})


// xoa danh muc
function deleteCate(id) {
    const url = 'https://localhost:7258/api/CateAPI/deleteCate/' + id;

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

