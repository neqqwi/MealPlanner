document.addEventListener('DOMContentLoaded', function() {
    const recipeForm = document.getElementById('recipeForm') || document.getElementById('recipeEditForm');
    if (!recipeForm) return;

    if (typeof $ !== 'undefined' && $.validator) {
        $.validator.setDefaults({ ignore: [] });
    }

    const ingredientsContainer = document.getElementById('ingredients-container');
    const ingredientsError = document.getElementById('ingredients-error');
    let originalOptionsHtml = '';
    const tomSelectInstances = new Map();
    let ingredientIndex = document.querySelectorAll('.ingredient-row').length;

    const firstSelect = document.querySelector('#ingredients-container select');
    if (firstSelect) {
        originalOptionsHtml = firstSelect.innerHTML;
    }

    if (window.TomSelect) {
        document.querySelectorAll('#ingredients-container select').forEach((select, index) => {
            initTomSelect(select, index);
        });
    }

    const fileInput = document.getElementById('imageFileInput');
    if (fileInput) {
        fileInput.addEventListener('change', handleFileChange);
        document.getElementById('clearFileButton')?.addEventListener('click', clearFile);
        document.getElementById('clearPreviewButton')?.addEventListener('click', clearFile);
    }

    document.getElementById('addIngredientBtn')?.addEventListener('click', addIngredientRow);

    ingredientsContainer?.addEventListener('click', function(e) {
        if (e.target.classList.contains('delete-btn')) {
            deleteIngredientRow(e);
        }
    });

    recipeForm.addEventListener('submit', function(e) {
        const rows = ingredientsContainer.querySelectorAll('.ingredient-row');
        if (rows.length === 0) {
            e.preventDefault();
            if (ingredientsError) {
                ingredientsError.classList.remove('hidden');
                ingredientsError.scrollIntoView({ behavior: 'smooth', block: 'center' });
            }
        }
    });

    function handleFileChange(e) {
        const file = e.target.files[0];
        const fileInfo = document.getElementById('fileInfo');
        const fileName = document.getElementById('fileName');
        const imagePreview = document.getElementById('imagePreview');
        const previewImage = document.getElementById('previewImage');
    
        let errorSpan = document.getElementById('imageFile-error');
        if (!errorSpan) {
            errorSpan = document.createElement('span');
            errorSpan.id = 'imageFile-error';
            errorSpan.className = 'text-sm text-red-500 mt-1 block';
            const fileInput = document.getElementById('imageFileInput');
            fileInput.parentNode.appendChild(errorSpan);
        }
    
        if (file) {
            const allowedExtensions = ['.jpg', '.jpeg', '.png']; 
            const ext = file.name.substring(file.name.lastIndexOf('.')).toLowerCase();
        
            if (!allowedExtensions.includes(ext)) {
                errorSpan.textContent = 'Можно загружать только JPG или PNG файлы';
                e.target.value = '';
                return;
            }
        
            const maxSize = 5 * 1024 * 1024; // 5 MB
            if (file.size > maxSize) {
                errorSpan.textContent = 'Размер файла не должен превышать 5MB';
                e.target.value = '';
                return;
            }
        
            if (!file.type.startsWith('image/')) {
                errorSpan.textContent = 'Файл должен быть изображением';
                e.target.value = '';
                return;
            }
        
            errorSpan.textContent = '';
            fileName.textContent = file.name;
            fileInfo.classList.remove('hidden');
            fileInfo.classList.add('flex');
        
            const reader = new FileReader();
            reader.onload = function(e) {
                previewImage.src = e.target.result;
                imagePreview.classList.remove('hidden');
                imagePreview.classList.add('block');
            };
            reader.readAsDataURL(file);
        } else {
            clearFile(); 
        }
    }

    function clearFile() {
        const fileInput = document.getElementById('imageFileInput');
        if (fileInput) fileInput.value = '';
    
        const fileInfo = document.getElementById('fileInfo');
        if (fileInfo) {
            fileInfo.classList.add('hidden');
            fileInfo.classList.remove('flex');
        }
    
        const imagePreview = document.getElementById('imagePreview');
        if (imagePreview) {
            imagePreview.classList.add('hidden');
            imagePreview.classList.remove('block');
        }
    
        const errorSpan = document.getElementById('imageFile-error');
        if (errorSpan) {
            errorSpan.textContent = '';
        }

        const currentImagePathInput = document.getElementById('CurrentImagePath');
        if (currentImagePathInput) {
            currentImagePathInput.value = '';
        }
    }

    function initTomSelect(selectElement, index) {
        if (!selectElement || !window.TomSelect) return;
        if (tomSelectInstances.has(index)) {
            tomSelectInstances.get(index).destroy();
            tomSelectInstances.delete(index);
        }
        const tomSelect = new TomSelect(selectElement, {
            create: false,
            sortField: { field: "text", direction: "asc" },
            placeholder: 'Поиск ингредиента...',
            searchField: ['text'],
            highlight: true,
            dropdownParent: 'body',
            maxOptions: null,
            onInitialize: function() {
                tomSelectInstances.set(index, this);
            },

            onChange: function(value) {
            const $select = $(selectElement);
            $select.val(value).trigger('change');
            if (value && value !== '') {
                $select.valid();
            }
        }
        });
        return tomSelect;
    }

    function addIngredientRow() {
        const row = document.createElement('div');
        row.className = 'ingredient-row';
        row.innerHTML = `
            <div class="grid grid-cols-1 gap-3 md:grid-cols-3 items-start">
                <div class="space-y-1">
                    <select name="RecipeIngredients[${ingredientIndex}].IngredientId" 
                            id="RecipeIngredients_${ingredientIndex}__IngredientId" 
                            required 
                            data-val="true" 
                            data-val-required="Выберите ингредиент из списка">
                        ${originalOptionsHtml}
                    </select>
                    <span class="text-sm text-red-500 field-validation-valid" 
                          data-valmsg-for="RecipeIngredients[${ingredientIndex}].IngredientId" 
                          data-valmsg-replace="true"></span>
                </div>
                <div class="space-y-1">
                    <input type="number" 
                           name="RecipeIngredients[${ingredientIndex}].Amount" 
                           id="RecipeIngredients_${ingredientIndex}__Amount" 
                           min="1" max="10000" value="1" required
                           data-val="true" 
                           data-val-required="Укажите количество" 
                           data-val-range="Количество должно быть от 1 до 10000" 
                           data-val-range-min="1" 
                           data-val-range-max="10000" 
                           data-val-number="Поле Количество должно быть числом"
                           class="w-full rounded-lg border border-slate-200 px-3 py-2 text-sm text-slate-900 placeholder-slate-400 transition focus:border-emerald-500 focus:ring-2 focus:ring-emerald-500/20 focus:outline-none" />
                    <span class="text-sm text-red-500 field-validation-valid" 
                          data-valmsg-for="RecipeIngredients[${ingredientIndex}].Amount" 
                          data-valmsg-replace="true"></span>
                </div>
                <button type="button" class="delete-btn rounded-lg border border-red-200 bg-red-50 px-3 py-2 text-sm font-medium text-red-600 transition hover:bg-red-100">
                    Удалить
                </button>
            </div>
        `;
        ingredientsContainer.appendChild(row);
        const newSelect = row.querySelector('select');
        newSelect.selectedIndex = 0;
    
        initTomSelect(newSelect, ingredientIndex);
        ingredientIndex++;
        reinitializeValidation();
        if (ingredientsError) ingredientsError.classList.add('hidden');
    }

    function deleteIngredientRow(e) {
        const row = e.target.closest('.ingredient-row');
        if (row) {
            row.remove();
            renumberIngredients();
        
            const rows = ingredientsContainer.querySelectorAll('.ingredient-row');
            if (rows.length === 0 && ingredientsError) {
                ingredientsError.classList.remove('hidden');
            }
        }
    }

    function renumberIngredients() {
        const rows = ingredientsContainer.querySelectorAll('.ingredient-row');
    
        const savedData = [];
        rows.forEach((row, index) => {
            const select = row.querySelector('select');
            const input = row.querySelector('input[type="number"]');
        
            const tomSelect = tomSelectInstances.get(index);
            savedData.push({
                selectValue: tomSelect ? tomSelect.getValue() : (select ? select.value : ''),
                amountValue: input ? input.value : '1'
            });
        });
    
        tomSelectInstances.forEach(instance => instance.destroy());
        tomSelectInstances.clear();
    
        rows.forEach((row, index) => {
            const select = row.querySelector('select');
            const input = row.querySelector('input[type="number"]');
        
            if (select) {
                select.name = `RecipeIngredients[${index}].IngredientId`;
                select.id = `RecipeIngredients_${index}__IngredientId`;
                select.value = savedData[index].selectValue;
                initTomSelect(select, index);
            }
            if (input) {
                input.name = `RecipeIngredients[${index}].Amount`;
                input.id = `RecipeIngredients_${index}__Amount`;
                input.value = savedData[index].amountValue;
            }
        });
    
        ingredientIndex = rows.length;
        reinitializeValidation();
    }
    function reinitializeValidation() {
        if (typeof $ !== 'undefined' && $.validator && $.validator.unobtrusive) {
            $(recipeForm).removeData('validator').removeData('unobtrusiveValidation');
            $.validator.unobtrusive.parse(recipeForm);
        }
    }
});