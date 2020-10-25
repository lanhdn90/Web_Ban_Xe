
$(document).ready(function(){
    CKEDITOR.replace("ChiTiet");

    $(function () {
        $('.datepicker').datepicker({
            dateFormat: "dd-mm-yy",
            changeMonth: true,
            changeYear: true,
            yearRange: '1970:2030',
            //showOn: "both",
            //buttonText: "<i class='fa fa-calendar'></i>"
        });
    });

    $("#SelectImage").click(function () {
        var finder = new CKFinder();
        finder.selectActionFunction = function (fileUrl) {
            $("#Anh").val(fileUrl);
        };
        finder.popup();
    });

    $(".xoaSanPham").off('click').on('click', function (e) {
        e.preventDefault();
        var id = $(this).data('id');
        var r = confirm('Bạn có muốn xóa' + id + ' hay không ?');
        if (r == true) {
            $.ajax({
                url: '/QLSanPham/Delete',
                data: { id: id },
                type: 'POST',
                success: function (data) {
                    var json = JSON.parse(data);
                    alert(json);
                    window.location.reload();
                },
                error: function (err) {
                    alert("Đã xảy ra lỗi" + err.responseText);
                }
            });
            //window.location.reload();
        }
    });

    $(".editCTHD").off('click').on('click', function (e) {
        e.preventDefault();
        var id = $(this).data('id1');
        var amount = $(this).data('amount');
        var r = prompt("Nhập số lượng muốn thay đổi","");
        if (r != null) {
            var soluong = parseInt(r);
            if (soluong > 0 && soluong <= amount) {
                $.ajax({
                    url: '/QLHoaDon/SuaChiTietHD',
                    data: {
                        id: id,
                        soluong: soluong
                    },
                    type: 'POST',
                    success: function (data) {
                        var json = JSON.parse(data);
                        alert(json);
                        window.location.reload();
                    },
                    error: function (err) {
                        alert("Đã xảy ra lỗi" + err.responseText);
                    }
                });
            } else {
                alert("Số lượng bạn mua vượt qua số lượng trong kho");
            }
            
        }
    });

    $(".xoaChiTietHD").off('click').on('click', function (e) {
        e.preventDefault();
        var id = $(this).data('id');
        var r = confirm('Bạn có muốn xóa' + id + ' hay không ?');
        if (r == true) {
            $.ajax({
                url: '/QLHoaDon/DeleteChiTietHD',
                data: { id: id },
                type: 'POST',
                success: function (data) {
                    var json = JSON.parse(data);
                    alert(json);
                    window.location.reload();
                },
                error: function (err) {
                    alert("Đã xảy ra lỗi" + err.responseText);
                }
            });
        }
    });

    $(".xoaHoaDon").off('click').on('click', function (e) {
        e.preventDefault();
        var id = $(this).data('id');
        var r = confirm('Bạn có muốn xóa' + id + ' hay không ?');
        if (r == true) {
            $.ajax({
                url: '/QLHoaDon/DeleteHD',
                data: { id: id },
                type: 'POST',
                success: function (data) {
                    var json = JSON.parse(data);
                    alert(json);
                    window.location.reload();
                },
                error: function (err) {
                    alert("Đã xảy ra lỗi" + err.responseText);
                }
            });
        }
    });

});