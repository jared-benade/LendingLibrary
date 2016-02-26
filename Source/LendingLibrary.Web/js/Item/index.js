var LendingLibrary = LendingLibrary || {};
LendingLibrary.Items = LendingLibrary.Items || {};
LendingLibrary.Items.Index = LendingLibrary.Items.Index || {};
(function (ns) {
    ns.deleteItem = function(id, deleteUrl, redirectUrl) {
        $.post(deleteUrl, { id: id }).then(function() {
            sweetAlert({
                    title: 'Item deleted successfully',
                    text: '',
                    type: 'success',
                    confirmButtonColor: '#7CB43F'
                },
                function() {
                    $.redirect(redirectUrl);
                });
        });
    };
})(LendingLibrary.Items.Index);