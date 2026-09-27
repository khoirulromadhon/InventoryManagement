$(function(){
    const apiList = '/api/SupplierAPI/GetAllSuppliers';
    const apiInsert = '/api/SupplierAPI/InsertSupplier';
    const apiDelete = '/api/SupplierAPI/DeleteSupplier/';

    let currentKeyword = '';
    let currentCursor = 0;
    const pageSize = 10;

    function load(keyword, cursor){
        currentKeyword = (typeof keyword === 'undefined') ? currentKeyword : keyword || '';
        currentCursor = (typeof cursor === 'undefined') ? currentCursor : cursor || 0;
        const params = [];
        if (currentKeyword) params.push(`keyword=${encodeURIComponent(currentKeyword)}`);
        params.push(`cursor=${currentCursor}`);
        const url = apiList + '?' + params.join('&');
        $.getJSON(url).done(function(data){
            if ((!data || data.length === 0) && currentCursor > 0) {
                load(currentKeyword, currentCursor - 1);
                return;
            }
            renderTable(data);
            renderPager(data.length, currentCursor, load);
        }).fail(function(){
            $('#suppliersMsg').html('<div class="alert alert-danger">Failed to load suppliers</div>');
        });
    }

    function renderTable(items){
        if(!items || items.length === 0){
            $('#suppliersTable').html('<div class="alert alert-secondary">No suppliers</div>');
            return;
        }
        let html = '<table class="table table-striped"><thead><tr><th>ID</th><th>Name</th><th>Action</th></tr></thead><tbody>';
        items.forEach(i => {
            html += `<tr><td>${i.supplierId ?? ''}</td><td>${i.supplierName ?? ''}</td><td><button class="btn btn-sm btn-danger delete-sup" data-id="${i.supplierId}">Delete</button></td></tr>`;
        });
        html += '</tbody></table>';
        $('#suppliersTable').html(html);
    }

    $('#addSupplier').on('click', function(){
        const name = $('#supplierName').val();
        if(!name){
            $('#suppliersMsg').html('<div class="alert alert-warning">Name required</div>');
            return;
        }
        $.ajax({
            url: apiInsert,
            method: 'POST',
            contentType: 'application/json',
            data: JSON.stringify({ supplierName: name }),
        }).done(function(){
            $('#suppliersMsg').html('<div class="alert alert-success">Added</div>');
            $('#supplierName').val('');
            load(currentKeyword, currentCursor);
        }).fail(function(){
            $('#suppliersMsg').html('<div class="alert alert-danger">Failed to add</div>');
        });
    });

    $('#searchSuppliers').on('click', function(){
        load($('#supplierSearch').val(), 0);
    });

    $('#suppliersTable').on('click', '.delete-sup', function(){
        const id = $(this).data('id');
        if(!confirm('Delete supplier?')) return;
        $.ajax({ url: apiDelete + id, method: 'DELETE' }).done(function(){
            load(currentKeyword, currentCursor);
        }).fail(function(){
            $('#suppliersMsg').html('<div class="alert alert-danger">Failed to delete</div>');
        });
    });

    load('', 0);

    function renderPager(itemCount, cursor, loadFn) {
        const pagerId = 'suppliersPager';
        let html = `<nav aria-label="Suppliers pager"><ul class="pagination">`;
        const prevDisabled = cursor <= 0 ? ' disabled' : '';
        html += `<li class="page-item${prevDisabled}"><a class="page-link" href="#" data-cursor="${cursor-1}">Previous</a></li>`;
        html += `<li class="page-item disabled"><span class="page-link">Page ${cursor + 1}</span></li>`;
        const nextDisabled = itemCount < pageSize ? ' disabled' : '';
        html += `<li class="page-item${nextDisabled}"><a class="page-link" href="#" data-cursor="${cursor+1}">Next</a></li>`;
        html += '</ul></nav>';
        if ($('#' + pagerId).length) {
            $('#' + pagerId).html(html);
        } else {
            $('#suppliersTable').append(`<div id="${pagerId}">` + html + `</div>`);
        }
        $('#' + pagerId + ' a.page-link').off('click').on('click', function(e){
            e.preventDefault();
            const target = $(this).data('cursor');
            if (target < 0) return;
            loadFn(currentKeyword, target);
        });
    }
});
