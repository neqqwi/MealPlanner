document.addEventListener('DOMContentLoaded', function () {
    const form = document.getElementById('productsForm');
    if (!form) return;

    if (typeof $ !== 'undefined' && $.validator) {
        $.validator.setDefaults({ ignore: [] });
    }

    const container = document.getElementById('products-container');
    const searchInput = document.getElementById('productSearch');
    const allIngredientsOptions = JSON.parse(document.getElementById('possible-ingredients-data').textContent);
    const tomSelectInstances = new Map();
    let rowIndex = document.querySelectorAll('.product-row').length;

    document.querySelectorAll('#products-container select').forEach((select, index) => {
        initTomSelect(select, index);
    });

    refreshAllSelectsOptions();

    document.getElementById('addProductBtn')?.addEventListener('click', addProductRow);

    container?.addEventListener('click', function (e) {
        if (e.target.classList.contains('delete-btn')) {
            deleteProductRow(e);
        }
    });

    searchInput?.addEventListener('input', function () {
        const term = this.value.trim().toLowerCase();
        container.querySelectorAll('.product-row').forEach(row => {
            const name = (row.dataset.name || '').toLowerCase();
            row.style.display = !term || name.includes(term) ? '' : 'none';
        });
    });

    function initTomSelect(selectElement, index) {
        if (!selectElement || !window.TomSelect) return;
        if (tomSelectInstances.has(index)) {
            tomSelectInstances.get(index).destroy();
            tomSelectInstances.delete(index);
        }

        const tomSelect = new TomSelect(selectElement, {
            create: false,
            sortField: { field: 'text', direction: 'asc' },
            placeholder: 'Поиск продукта...',
            searchField: ['text'],
            highlight: true,
            dropdownParent: 'body',
            maxOptions: null,
            onInitialize: function () {
                tomSelectInstances.set(index, this);
                updateRowName(selectElement);
            },
            onChange: function (value) {
                const $select = $(selectElement);
                $select.val(value).trigger('change');
                if (value && value !== '') {
                    $select.valid();
                }
                updateRowName(selectElement);
                refreshAllSelectsOptions();
            }
        });

        return tomSelect;
    }

    function updateRowName(selectElement) {
        const row = selectElement.closest('.product-row');
        if (!row) return;
        const instance = selectElement.tomselect;
        const text = instance && instance.getValue()
            ? instance.options[instance.getValue()]?.text ?? ''
            : '';
        row.dataset.name = text;
    }

    function getSelectedIdsExcept(excludeSelect) {
        const ids = new Set();
        container.querySelectorAll('select').forEach(sel => {
            if (sel !== excludeSelect && sel.value) {
                ids.add(sel.value);
            }
        });
        return ids;
    }

    function refreshOptionsForSelect(selectElement) {
        const instance = selectElement.tomselect;
        if (!instance) return;

        const usedElsewhere = getSelectedIdsExcept(selectElement);

        allIngredientsOptions.forEach(opt => {
            const isUsedElsewhere = usedElsewhere.has(opt.value);
            const alreadyInDropdown = !!instance.options[opt.value];

            if (isUsedElsewhere && alreadyInDropdown && instance.getValue() !== opt.value) {
                instance.removeOption(opt.value);
            } else if (!isUsedElsewhere && !alreadyInDropdown) {
                instance.addOption(opt);
            }
        });

        instance.refreshOptions(false);
    }

    function refreshAllSelectsOptions() {
        container.querySelectorAll('select').forEach(refreshOptionsForSelect);
    }

    function addProductRow() {
        const row = document.createElement('tr');
        row.className = 'product-row group transition hover:bg-slate-50';
        row.innerHTML = `
            <td class="w-5/12 px-6 py-4 align-top">
                <select name="Products[${rowIndex}].IngredientId"
                        id="Products_${rowIndex}__IngredientId"
                        required data-val="true" data-val-required="Выберите продукт">
                </select>
                <span class="text-sm text-red-500 field-validation-valid"
                      data-valmsg-for="Products[${rowIndex}].IngredientId" data-valmsg-replace="true"></span>
            </td>
            <td class="w-48 px-6 py-4 text-center align-top">
                <div class="inline-flex flex-col items-center gap-1">
                    <input type="number" name="Products[${rowIndex}].Quantity"
                           id="Products_${rowIndex}__Quantity" min="0" max="100000" value="1" required
                           data-val="true" data-val-required="Укажите количество"
                           data-val-range="Количество не может быть отрицательным" data-val-range-min="0" data-val-range-max="100000"
                           class="w-24 rounded-lg border border-slate-200 px-3 py-2 text-center text-sm text-slate-900 transition focus:border-emerald-500 focus:ring-2 focus:ring-emerald-500/20 focus:outline-none" />
                    <span class="block text-xs text-red-500 field-validation-valid"
                          data-valmsg-for="Products[${rowIndex}].Quantity" data-valmsg-replace="true"></span>
                </div>
            </td>
            <td class="w-48 px-6 py-4 text-center align-top">
                <button type="button" class="delete-btn rounded-xl px-3 py-1.5 text-sm font-medium text-red-600 transition hover:bg-red-50">
                    Удалить
                </button>
            </td>
        `;
        container.appendChild(row);

        const newSelect = row.querySelector('select');
        initTomSelect(newSelect, rowIndex);
        refreshAllSelectsOptions();

        rowIndex++;
        reinitializeValidation();
    }

    function deleteProductRow(e) {
        const row = e.target.closest('.product-row');
        if (row) {
            row.remove();
            renumberRows();
        }
    }

    function renumberRows() {
        const rows = container.querySelectorAll('.product-row');

        const savedData = [];
        rows.forEach(row => {
            const select = row.querySelector('select');
            const input = row.querySelector('input[type="number"]');
            const tomSelect = select ? select.tomselect : null;

            savedData.push({
                selectValue: tomSelect ? tomSelect.getValue() : (select ? select.value : ''),
                quantityValue: input ? input.value : '1'
            });
        });

        tomSelectInstances.forEach(instance => instance.destroy());
        tomSelectInstances.clear();

        rows.forEach((row, index) => {
            const select = row.querySelector('select');
            const input = row.querySelector('input[type="number"]');

            if (select) {
                select.name = `Products[${index}].IngredientId`;
                select.id = `Products_${index}__IngredientId`;
                initTomSelect(select, index);
                select.tomselect.setValue(savedData[index].selectValue, true);
            }
            if (input) {
                input.name = `Products[${index}].Quantity`;
                input.id = `Products_${index}__Quantity`;
                input.value = savedData[index].quantityValue;
            }
        });

        rowIndex = rows.length;
        refreshAllSelectsOptions();
        reinitializeValidation();
    }

    function reinitializeValidation() {
        if (typeof $ !== 'undefined' && $.validator && $.validator.unobtrusive) {
            $(form).removeData('validator').removeData('unobtrusiveValidation');
            $.validator.unobtrusive.parse(form);
        }
    }
});