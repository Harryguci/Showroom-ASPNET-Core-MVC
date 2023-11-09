function HandleDelete(event) {
    event.preventDefault();
    if (confirm("Bạn có chắc muốn xóa?")) {
        var id = document.querySelector("#deleteForm input").value.trim();
        document.getElementById("deleteForm").submit();
        // console.log(id);
        //$.ajax({
        //    method: "POST",
        //    url: `/Customers/DeleteConfirmed`,
        //    data: { ClientId: id },
        //    dataType: "json",
        //    success: function (response) {
        //        window.location = "/Customers/Trash";
        //    },
        //});
    }
}

function HandleUndoFromTrash(event) {
    event.preventDefault();
    document.getElementById("undoForm").submit();
}
