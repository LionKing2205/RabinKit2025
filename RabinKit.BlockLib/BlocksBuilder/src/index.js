// Import Blockly core.
import * as Blockly from 'blockly/core.js';
// Import the default blocks.
import * as libraryBlocks from 'blockly/blocks.js';
// Import a generator.
import { pythonGenerator, Order } from 'blockly/python.js';
// Import a message file.
import * as Ru from 'blockly/msg/ru.js';

Blockly.setLocale(Ru);

export const init = (toolbox, playgroundSource, parameters) => {
    console.log('Init');

    var playground = JSON.parse(playgroundSource);

    const blocklyArea = document.getElementById('blocklyArea');
    const blocklyDiv = document.getElementById('blocklyDiv');

    const theme = Blockly.Theme.defineTheme('dark', {
        base: Blockly.Themes.Classic,
        componentStyles: {
            workspaceBackgroundColour: '#1e1e1e',
            toolboxBackgroundColour: 'blackBackground',
            toolboxForegroundColour: '#fff',
            flyoutBackgroundColour: '#252526',
            flyoutForegroundColour: '#ccc',
            flyoutOpacity: 1,
            scrollbarColour: '#797979',
            insertionMarkerColour: '#fff',
            insertionMarkerOpacity: 0.3,
            scrollbarOpacity: 0.4,
            cursorColour: '#d0d0d0',
            blackBackground: '#333',
        },
    })

    Blockly.defineBlocksWithJsonArray([
        {
            "type": "crc32",
            "message0": "CRC32 от %1",
            "args0": [
                {
                    "type": "input_value",
                    "name": "val"
                }
            ],
            "inputsInline": true,
            "output": "Number",
            "colour": 200,
            "tooltip": "CRC32",
            "helpUrl": ""
        },
        {
            "type": "pow_mod",
            "message0": "%1 ^ %2 mod %3",
            "args0": [
                {
                    "type": "input_value",
                    "name": "base",
                    "check": "Number"
                },
                {
                    "type": "input_value",
                    "name": "exponent",
                    "check": "Number"
                },
                {
                    "type": "input_value",
                    "name": "modulo",
                    "check": "Number"
                }
            ],
            "inputsInline": true,
            "output": "Number",
            "colour": 200,
            "tooltip": "Нахождение степени по модулю",
            "helpUrl": ""
        },
        {
            "type": "gcd",
            "message0": "НОД %1, %2",
            "args0": [
                {
                    "type": "input_value",
                    "name": "value1",
                    "check": "Number"
                },
                {
                    "type": "input_value",
                    "name": "value2",
                    "check": "Number"
                }
            ],
            "inputsInline": true,
            "output": "Number",
            "colour": 200,
            "tooltip": "Нахождение наибольшего общего делителя",
            "helpUrl": ""
        },
        {
            "type": "factorint",
            "message0": "Получить список простых множителей для %1",
            "args0": [
                {
                    "type": "input_value",
                    "name": "value",
                    "check": "Number"
                },
            ],
            "inputsInline": true,
            "output": "Number",
            "colour": 200,
            "tooltip": "Нахождение наибольшего общего делителя",
            "helpUrl": ""
        },
        {
            "type": "dict_create",
            "message0": "пустой словарь",
            "output": "dict",
            "colour": 230,
            "tooltip": "",
            "helpUrl": ""
        },
        {
            "type": "dict_set",
            "message0": "в словаре %1 установить по ключу %2 значение %3",
            "args0": [
                {
                    "type": "input_value",
                    "name": "dictVar",
                    "check": "dict"
                },
                {
                    "type": "input_value",
                    "name": "key"
                },
                {
                    "type": "input_value",
                    "name": "val"
                }
            ],
            "inputsInline": true,
            "previousStatement": null,
            "nextStatement": null,
            "colour": 300,
            "tooltip": "",
            "helpUrl": ""
        },
        {
            "type": "dict_get",
            "message0": "из словаря %1 вернуть по ключу %2",
            "args0": [
                {
                    "type": "input_value",
                    "name": "dict",
                    "check": "dict"
                },
                {
                    "type": "input_value",
                    "name": "key"
                }
            ],
            "inputsInline": true,
            "output": null,
            "colour": 150,
            "tooltip": "",
            "helpUrl": ""
        },
        {
            "type": "dict_get_or_default",
            "message0": "из словаря %1 вернуть по ключу %2 или %3",
            "args0": [
                {
                    "type": "input_value",
                    "name": "dict",
                    "check": "dict"
                },
                {
                    "type": "input_value",
                    "name": "key"
                },
                {
                    "type": "input_value",
                    "name": "default"
                }
            ],
            "inputsInline": true,
            "output": null,
            "colour": 150,
            "tooltip": "",
            "helpUrl": ""
        },
        {
            "type": "dict_keys",
            "message0": "из словаря %1 вернуть ключи",
            "args0": [
                {
                    "type": "input_value",
                    "name": "dict",
                    "check": "dict",
                    "align": "CENTRE"
                }
            ],
            "output": "Array",
            "colour": 230,
            "tooltip": "",
            "helpUrl": ""
        },
        {
            "type": "dict_values",
            "message0": "из словаря %1 вернуть значения",
            "args0": [
                {
                    "type": "input_value",
                    "name": "dict",
                    "check": "dict",
                    "align": "CENTRE"
                }
            ],
            "output": "Array",
            "colour": 230,
            "tooltip": "",
            "helpUrl": ""
        },
        {
            "type": "isqrt",
            "message0": "целый квадратный корень %1",
            "args0": [
                {
                    "type": "input_value",
                    "name": "val"
                }
            ],
            "output": null,
            "colour": 230,
            "tooltip": "",
            "helpUrl": ""
        },
        {
            "type": "to_int",
            "message0": "целое %1",
            "args0": [
                {
                    "type": "input_value",
                    "name": "val",
                    "check": "Number"
                }
            ],
            "output": "Number",
            "colour": 230,
            "tooltip": "преобразовать число в целое",
            "helpUrl": ""
        },
        {
            "type": "abs",
            "message0": "модуль %1",
            "args0": [
                {
                    "type": "input_value",
                    "name": "val",
                    "check": "Number"
                }
            ],
            "output": "Number",
            "colour": 230,
            "tooltip": "целочисленный модуль",
            "helpUrl": ""
        },
        {
            "type": "minimum",
            "message0": "наименьшее %1 %2",
            "args0": [
                {
                    "type": "input_value",
                    "name": "val1",
                    "check": "Number"
                },
                {
                    "type": "input_value",
                    "name": "val2",
                    "check": "Number"
                }
            ],
            "output": "Number",
            "colour": 230,
            "tooltip": "",
            "helpUrl": ""
        },
        {
            "type": "getrandbits_4_3",
            "message0": "случайное число с длиной бит %1",
            "args0": [
                {
                    "type": "input_value",
                    "name": "bit_length",
                    "check": "Number"
                }
            ],
            "inputsInline": true,
            "output": "Number",
            "colour": 200,
            "tooltip": "Генерирует случайное число с указанной длиной бит и добавляет 0b11.",
            "helpUrl": ""
        },
        {
            "type": "decryption_defs",
            "message0": "p = %1,  q = %2, c = %3, m_id = %4",
            "args0": [
                {
                    "type": "input_value",
                    "name": "p_key",
                    "check": "Number"
                },
                {
                    "type": "input_value",
                    "name": "q_key",
                    "check": "Number"
                },
                {
                    "type": "input_value",
                    "name": "enc_mess",
                    "check": "Number"
                },
                {
                    "type": "input_value",
                    "name": "mess_id",
                    "check": "Number"
                }
            ],
            "inputsInline": true,
            "output": "Number",
            "colour": 200,
            "tooltip": "Расшифровать сообщение получить указанный вариант расшифровки (от 1 до 4)",
            "helpUrl": ""
        },
        {
            "type": "decryption_defs_list",
            "message0": "p = %1,  q = %2, c = %3",
            "args0": [
                {
                    "type": "input_value",
                    "name": "p_key",
                    "check": "Number"
                },
                {
                    "type": "input_value",
                    "name": "q_key",
                    "check": "Number"
                },
                {
                    "type": "input_value",
                    "name": "enc_mess",
                    "check": "Number"
                }
            ],
            "inputsInline": true,
            "output": "Array",
            "colour": 190,
            "tooltip": "Расшифровать сообщение получить списком все варианты расшифровки",
            "helpUrl": ""
        }
    ]);
    pythonGenerator.forBlock['to_int'] = function (block, generator) {
        var _value = generator.valueToCode(block, 'val', Order.ATOMIC);
        var code = 'int(' + _value + ')';
        return [code, Order.FUNCTION_CALL];
    };

    pythonGenerator.forBlock['abs'] = function (block, generator) {
        var _value = generator.valueToCode(block, 'val', Order.ATOMIC);
        var code = 'abs(' + _value + ')';
        return [code, Order.FUNCTION_CALL];
    };

    pythonGenerator.forBlock['minimum'] = function (block, generator) {
        var a = generator.valueToCode(block, 'val1', Order.ATOMIC);
        var b = generator.valueToCode(block, 'val2', Order.ATOMIC);
        var code = 'min('+a+','+b+')';
        return [code, Order.FUNCTION_CALL];
    };

    pythonGenerator.forBlock['decryption_defs'] = function (block, generator) {
        var _p_key = generator.valueToCode(block, 'p_key', Order.ATOMIC);
        var _q_key = generator.valueToCode(block, 'q_key', Order.ATOMIC);
        var _c = generator.valueToCode(block, 'enc_mess', Order.ATOMIC);
        var _mess_id = generator.valueToCode(block, 'mess_id', Order.ATOMIC);
        var code = 'decryption_mod(' + _p_key + ', ' + _q_key + ', ' + _c + ', ' + _mess_id + ')';
        return [code, Order.FUNCTION_CALL];
    };

    pythonGenerator.forBlock['decryption_defs_list'] = function (block, generator) {
        var _p_key = generator.valueToCode(block, 'p_key', Order.ATOMIC);
        var _q_key = generator.valueToCode(block, 'q_key', Order.ATOMIC);
        var _c = generator.valueToCode(block, 'enc_mess', Order.ATOMIC);
        var code = 'decryption_mod_list(' + _p_key + ', ' + _q_key + ', ' + _c + ')';
        return [code, Order.FUNCTION_CALL];
    };

    pythonGenerator.forBlock['getrandbits_4_3'] = function (block, generator) {
        generator.definitions_['import_random'] = 'import random';
        var value_bit_length = pythonGenerator.valueToCode(block, 'bit_length', pythonGenerator.ORDER_ATOMIC);
        var code = 'random.getrandbits(int(' + value_bit_length + ') - 2) | (1 << (int(' + value_bit_length +') - 1)) | 0b11';
        return [code, pythonGenerator.ORDER_NONE];
    };

    pythonGenerator.forBlock['crc32'] = function (block, generator) {
        generator.definitions_['import_zlib'] = 'import zlib';
        var value_message = pythonGenerator.valueToCode(block, 'val', pythonGenerator.ORDER_ATOMIC);
        var code = 'zlib.crc32(str(int(' + value_message + ')).encode("utf-8")) & 0xffffffff';
        return [code, pythonGenerator.ORDER_NONE];
    };

    pythonGenerator.forBlock['pow_mod'] = function (block, generator) {
        var value_base = generator.valueToCode(block, 'base', Order.ATOMIC);
        var value_exponent = generator.valueToCode(block, 'exponent', Order.ATOMIC);
        var value_name = generator.valueToCode(block, 'modulo', Order.ATOMIC);
        var code = 'pow(' + value_base + ', ' + value_exponent + ', ' + value_name + ')';
        return [code, Order.FUNCTION_CALL];
    };

    pythonGenerator.forBlock['dict_create'] = function (block, generator) {
        var code = '{}';
        return [code, Order.COLLECTION];
    };

    pythonGenerator.forBlock['dict_set'] = function (block, generator) {
        var value_dictvar = generator.valueToCode(block, 'dictVar', Order.ATOMIC);
        var value_key = generator.valueToCode(block, 'key', Order.ATOMIC);
        var value_val = generator.valueToCode(block, 'val', Order.ATOMIC);
        // TODO: Assemble python into code variable.
        var code = `${value_dictvar}[${value_key}] = ${value_val}\n`;
        return code;
    };

    pythonGenerator.forBlock['dict_get'] = function (block, generator) {
        var value_dict = generator.valueToCode(block, 'dict', Order.ATOMIC);
        var value_key = generator.valueToCode(block, 'key', Order.ATOMIC);

        var code = `${value_dict}[${value_key}]`;
        return [code, Order.COLLECTION];
    };

    pythonGenerator.forBlock['dict_get_or_default'] = function (block, generator) {
        var value_dict = generator.valueToCode(block, 'dict', Order.ATOMIC);
        var value_key = generator.valueToCode(block, 'key', Order.ATOMIC);
        var value_default = generator.valueToCode(block, 'default', Order.ATOMIC);

        var code = `${value_dict}.get(${value_key}, ${value_default})`;
        // TODO: Change ORDER_NONE to the correct strength.
        return [code, Order.FUNCTION_CALL];
    };

    pythonGenerator.forBlock['isqrt'] = function (block, generator) {
        generator.definitions_['import_math'] = 'import math';

        var value_val = generator.valueToCode(block, 'val', Order.ATOMIC);

        var code = `int(math.sqrt(${value_val}))`;
        return [code, Order.FUNCTION_CALL];
    };

    pythonGenerator.forBlock['gcd'] = function (block, generator) {
        generator.definitions_['import_math'] = 'import math';

        var value_val1 = generator.valueToCode(block, 'value1', Order.ATOMIC);
        var value_val2 = generator.valueToCode(block, 'value2', Order.ATOMIC);

        var code = `math.gcd(${value_val1}, ${value_val2})`;
        return [code, Order.FUNCTION_CALL];
    };

    pythonGenerator.forBlock['factorint'] = function (block, generator) {
        generator.definitions_['import_sympy_factorint'] = 'from sympy import factorint';

        var value_val = generator.valueToCode(block, 'value', Order.ATOMIC);

        var code = `factorint(${value_val})`;
        return [code, Order.FUNCTION_CALL];
    };

    pythonGenerator.forBlock['dict_keys'] = function (block, generator) {
        var value_dict = generator.valueToCode(block, 'dict', Order.ATOMIC);
        var code = `list(${value_dict}.keys())`;
        return [code, Order.FUNCTION_CALL];
    };

    pythonGenerator.forBlock['dict_values'] = function (block, generator) {
        var value_dict = generator.valueToCode(block, 'dict', Order.ATOMIC);
        var code = `list(${value_dict}.values())`;
        return [code, Order.FUNCTION_CALL];
    };

    let workspace = Blockly.inject(blocklyDiv, {
        toolbox: toolbox,
        theme: theme
    })

    parameters.map(item => {
        workspace.createVariable(item);
    })

    const onresize = function (e) {
        // Compute the absolute coordinates and dimensions of blocklyArea.
        let element = blocklyArea;
        let x = 0;
        let y = 0;
        do {
            x += element.offsetLeft;
            y += element.offsetTop;
            element = element.offsetParent;
        } while (element);
        // Position blocklyDiv over blocklyArea.
        blocklyDiv.style.left = x + 'px';
        blocklyDiv.style.top = y + 'px';
        blocklyDiv.style.width = blocklyArea.offsetWidth + 'px';
        blocklyDiv.style.height = blocklyArea.offsetHeight + 'px';
        Blockly.svgResize(workspace);
    };
    window.addEventListener('resize', onresize, false);
    onresize();

    if (playground)
        Blockly.serialization.workspaces.load(playground, workspace);
    console.log(workspace);
    return workspace;
};

export const generate = (workspace) => {
    return pythonGenerator.workspaceToCode(workspace)
};

export const save = (workspace) => {
    return JSON.stringify(Blockly.serialization.workspaces.save(workspace))
};