$(() => {
    $(document).ready(() => {
        $('.table').DataTable({
            ajax: 'http://localhost:21000/api/Products',
            columns: [
                {
                    data: 'processes',
                    render: function (data, type, full, meta) {
                        var innerHtml = '';

                        innerHtml += '<button class="btn btn-sm btn-outline-danger btn-delete mr-3" data-id="';
                        innerHtml += full.id;
                        innerHtml += '" data-name="';
                        innerHtml += full.name;
                        innerHtml += '" data-unitPrice="';
                        innerHtml += full.unitPrice;
                        innerHtml += '" data-quantityInStock="';
                        innerHtml += full.quantityInStock;
                        innerHtml += '" data-toggle="modal" data-target="#deleteModal">Sil</button>';
                        innerHtml += '<button class="btn btn-sm btn-outline-secondary btn-edit" data-id="';
                        innerHtml += full.id;
                        innerHtml += '" data-name="';
                        innerHtml += full.name;
                        innerHtml += '" data-unitPrice="';
                        innerHtml += full.unitPrice;
                        innerHtml += '" data-quantityInStock="';
                        innerHtml += full.quantityInStock;
                        innerHtml += '" data-toggle="modal" data-target="#editModal">Düzenle</button>';

                        return innerHtml;
                    }
                },
                { data: 'id' },
                { data: 'name' },
                { data: 'unitPrice' },
                { data: 'quantityInStock' }
            ],
            autoWidth: false,
            columnDefs: [
                {
                    targets: 0,
                    orderable: false,
                    searchable: false,
                    className: 'td-process-column'
                },
                {
                    targets: 1,
                    className: 'td-order-column'
                }
            ],
            language: {
                "lengthMenu": "Sayfada _MENU_ kayıt gösteriliyor",
                "zeroRecords": "Görüntülenecek kayıt bulunamadı",
                "info": "_TOTAL_ kayıttan _START_ - _END_ arasındaki kayıtlar gösteriliyor",
                "infoEmpty": "Kayıt yok",
                "infoFiltered": "(_MAX_ kayıt içerisinden bulunan)",
                "search": "Ara:",
                "paginate": {
                    "first": "İlk",
                    "last": "Son",
                    "next": "Sonraki",
                    "previous": "Önceki"
                }
            },
            lengthMenu: [
                [10, 25, 50, 100],
                [10, 25, 50, 100],
            ],
            order: [[1, 'asc']],
            ordering: true,
            scrollX: true,
            searching: true
        });

        $('.table').on('draw.dt', function () {
            btnDelete_bindClickEvent();
            btnEdit_bindClickEvent();
        });

        function btnDelete_bindClickEvent() {
            $('.btn-delete').on('click', (e) => {
                var dataId = $(e.target).data('id');
                var dataName = $(e.target).data('name');
                var dataUnitPrice = $(e.target).data('unitprice');
                var dataQuantityInStock = $(e.target).data('quantityinstock');
                $('#mdlDeleteId').attr('value', dataId);
                $('#mdlDeleteId').attr('value', dataId);
                $('#mdlDeleteName').attr('value', dataName);
                $('#mdlDeleteUnitPrice').attr('value', dataUnitPrice);
                $('#mdlDeleteQuantityInStock').attr('value', dataQuantityInStock);
            });
        }

        btnDelete_bindClickEvent();

        btnEdit_bindClickEvent();
    });
})