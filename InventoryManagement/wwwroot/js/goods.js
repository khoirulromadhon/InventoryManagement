$(function(){
    const apiList = '/api/GoodAPI/GetAllGoods';

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
            $('#goodsMsg').html('<div class="alert alert-danger">Failed to load goods</div>');
        });
    }

    function renderTable(items){
        if(!items || items.length === 0){
            $('#goodsTable').html('<div class="alert alert-secondary">No goods</div>');
            return;
        }
        let html = '<table class="table table-striped"><thead><tr><th>Code</th><th>Name</th><th>Category</th><th>Supplier</th><th>Stock</th></tr></thead><tbody>';
        items.forEach(i => {
            html += `<tr><td>${i.goodCode ?? ''}</td><td>${i.goodName ?? ''}</td><td>${i.categoryName ?? ''}</td><td>${i.supplierName ?? ''}</td><td>${i.goodStock ?? ''}</td></tr>`;
        });
        html += '</tbody></table>';
        $('#goodsTable').html(html);
    }

    $('#searchGoods').on('click', function(){
        load($('#goodsSearch').val(), 0);
    });

    $('#refreshGoods').on('click', function(){
        load('', 0);
    });

    load('', 0);

    function renderPager(itemCount, cursor, loadFn) {
        const pagerId = 'goodsPager';
        let html = `<nav aria-label="Goods pager"><ul class="pagination">`;
        const prevDisabled = cursor <= 0 ? ' disabled' : '';
        html += `<li class="page-item${prevDisabled}"><a class="page-link" href="#" data-cursor="${cursor-1}">Previous</a></li>`;
        html += `<li class="page-item disabled"><span class="page-link">Page ${cursor + 1}</span></li>`;
        const nextDisabled = itemCount < pageSize ? ' disabled' : '';
        html += `<li class="page-item${nextDisabled}"><a class="page-link" href="#" data-cursor="${cursor+1}">Next</a></li>`;
        html += '</ul></nav>';
        if ($('#' + pagerId).length) {
            $('#' + pagerId).html(html);
        } else {
            $('#goodsTable').append(`<div id="${pagerId}">` + html + `</div>`);
        }
        $('#' + pagerId + ' a.page-link').off('click').on('click', function(e){
            e.preventDefault();
            const target = $(this).data('cursor');
            if (target < 0) return;
            loadFn(currentKeyword, target);
        });
    }
});
