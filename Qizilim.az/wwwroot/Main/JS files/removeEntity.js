function removeEntity(ev, entityId, name) {
    console.log("Salma")
    swal({
        title: "Əminsiniz?",
        text: `${name} adlı qeyd silinəcək.`,
        icon: "warning",
        buttons: true,
        dangerMode: true,
    })
        .then((willDelete) => {
            if (willDelete) {
                $.ajax({
                    url: `@Url.Action("DeleteProduct","Account")/${entityId}`,
                    type: 'post',
                    success: function (response) {
                        if (response.error == true) {
                            toastr.error(response.message, "Xəta!");
                            return;
                        }
                        toastr["success"](response.message, "Ugurludur!");
                        $(`tr[data-entity-id=${entityId}]`).remove();
                        location.pathname = "/account/myStore";
                    },
                    error: function (response) {
                        //console.warn(response);
                    }
                });
            }
        });
}