$(function(){
    const apiMutate = '/api/GoodMutationAPI/Mutation';
    const apiHistory = '/api/GoodMutationAPI/MutationHistory';

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
        let html = '<table class="table table-striped"><thead><tr><th>ID</th><th>Good</th><th>Type</th><th>Amount</th><th>Note</th><th>Date</th></tr></thead><tbody>';
        items.forEach(i => {
            html += `<tr><td>${i.mutationId ?? ''}</td><td>${i.goodName ?? ''}</td><td>${i.status ?? ''}</td><td>${i.amount ?? ''}</td><td>${i.note ?? ''}</td><td>${i.mutationDate ?? ''}</td></tr>`;
        });
        html += '</tbody></table>';
        $('#mutationsTable').html(html);
    }

    $('#doMutation').on('click', function(){
        const goodId = parseInt($('#mutationGoodId').val());
        const type = $('#mutationType').val();
        const amount = parseInt($('#mutationAmount').val());
        if(!goodId || !amount){
            $('#mutationsMsg').html('<div class="alert alert-warning">Good and amount required</div>');
            return;
        }
        const payload = {
            GoodId: goodId,
            Status: type,
            Amount: amount
        };
        $.ajax({ url: apiMutate, method: 'POST', contentType: 'application/json', data: JSON.stringify(payload) })
            .done(function(){
                $('#mutationsMsg').html('<div class="alert alert-success">Mutation success</div>');
                load();
            }).fail(function(){
                $('#mutationsMsg').html('<div class="alert alert-danger">Mutation failed</div>');
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
