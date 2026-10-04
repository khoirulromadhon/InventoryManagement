$(function(){
    const apiMutate = '/api/GoodMutationAPI/Mutation';
    const apiHistory = '/api/GoodMutationAPI/MutationHistory';
    const apiGoods = '/api/GoodAPI/GetAllGoods';
    const apiCategories = '/api/CategoryAPI/GetAllCategories';
    const apiSuppliers = '/api/SupplierAPI/GetAllSuppliers';

    function load(keyword){
        const url = apiHistory + (keyword ? `?keyword=${encodeURIComponent(keyword)}` : '');
        $.getJSON(url).done(function(data){
            renderTable(data);
        }).fail(function(){
            $('#mutationsMsg').html('<div class="alert alert-danger">Failed to load history</div>');
        });
    }

    function renderTable(items){
        if(!items || items.length === 0){
            $('#mutationsTable').html('<div class="alert alert-secondary">No history</div>');
            return;
        }
        let html = '<table class="table table-striped"><thead><tr><th>Good</th><th>Type</th><th>Amount</th><th>Date</th></tr></thead><tbody>';
        items.forEach(i => {
            html += `<tr><td>${i.goodName ?? ''}</td><td>${i.status ?? ''}</td><td>${i.amount ?? ''}</td><td>${i.mutationDate ?? ''}</td></tr>`;
        });
        html += '</tbody></table>';
        $('#mutationsTable').html(html);
    }

    function loadDropdowns(){
        // Goods
        $.getJSON(apiGoods + '?cursor=0').done(function(data){
            const sel = $('#mutationGoodIdSelect');
            sel.empty();
            sel.append('<option value="">-- Select Good --</option>');
            data.forEach(g => sel.append(`<option value="${g.goodName}">${g.goodName} (${g.goodCode})</option>`));
        });

        // Categories
        $.getJSON(apiCategories + '?cursor=0').done(function(data){
            const sel = $('#mutationCategoryIdSelect');
            sel.empty();
            sel.append('<option value="">-- Select Category --</option>');
            data.forEach(c => sel.append(`<option value="${c.categoryId}">${c.categoryName}</option>`));
        });

        // Suppliers
        $.getJSON(apiSuppliers + '?cursor=0').done(function(data){
            const sel = $('#mutationSupplierIdSelect');
            sel.empty();
            sel.append('<option value="">-- Select Supplier --</option>');
            data.forEach(s => sel.append(`<option value="${s.supplierId}">${s.supplierName}</option>`));
        });
    }

    $('#openMutationModal').on('click', function(){
        loadDropdowns();
        $('#mutationModalMsg').html('');
        $('#mutationAmountInput').val('');
        var modal = new bootstrap.Modal(document.getElementById('mutationModal'));
        modal.show();
    });

    $('#saveMutation').on('click', function(){
        const goodName = $('#mutationGoodIdSelect').val();
        const categoryId = parseInt($('#mutationCategoryIdSelect').val());
        const supplierId = parseInt($('#mutationSupplierIdSelect').val());
        const type = $('#mutationTypeSelect').val();
        const amount = parseInt($('#mutationAmountInput').val());

        if (type == 'INBOUND') {
            if (!goodName || !categoryId || !supplierId || !amount) {
                $('#mutationModalMsg').html('<div class="alert alert-warning">All fields required</div>');
                return;
            }
        }
        else if (type == 'OUTBOUND') {
            if (!goodName || !categoryId || !supplierId || !amount) {
                $('#mutationModalMsg').html('<div class="alert alert-warning">All fields required</div>');
                return;
            }
        }
        else {
            $('#mutationModalMsg').html('<div class="alert alert-warning">Invalid mutation type</div>');
            return;
        }

        const payload = {
            GoodName: goodName,
            CategoryId: categoryId,
            SupplierId: supplierId,
            Status: type,
            Amount: amount
        };

        $.ajax({ url: apiMutate, method: 'POST', contentType: 'application/json', data: JSON.stringify(payload) })
            .done(function(){
                $('#mutationsMsg').html('<div class="alert alert-success">Mutation success</div>');
                load();
                var modalEl = document.getElementById('mutationModal');
                var modal = bootstrap.Modal.getInstance(modalEl);
                modal.hide();
            }).fail(function(){
                $('#mutationModalMsg').html('<div class="alert alert-danger">Mutation failed</div>');
            });
    });

    $('#searchMutations').on('click', function(){
        load($('#mutSearch').val());
    });

    $('#refreshMutations').on('click', function(){
        load();
    });

    load();
});
