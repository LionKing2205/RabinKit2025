export const toolbox1 =  {
    "kind": "categoryToolbox",
    "contents": [
        {
            "kind": "category",
            "name": "Логика",
            "categorystyle": "logic_category",
            "contents": [
                {
                    "kind": "category",
                    "name": "Если",
                    "contents": [
                        {
                            "kind": "block",
                            "type": "controls_if"
                        },
                        {
                            "kind": "block",
                            "type": "controls_if",
                            "extraState": {
                                "hasElse": "true"
                            }
                        },
                        {
                            "kind": "block",
                            "type": "controls_if",
                            "extraState": {
                                "hasElse": "true",
                                "elseIfCount": 1
                            }
                        }
                    ]
                },
                {
                    "kind": "category",
                    "name": "Булево",
                    "categorystyle": "logic_category",
                    "contents": [
                        {
                            "kind": "block",
                            "type": "logic_compare"
                        },
                        {
                            "kind": "block",
                            "type": "logic_operation"
                        },
                        {
                            "kind": "block",
                            "type": "logic_negate"
                        },
                        {
                            "kind": "block",
                            "type": "logic_boolean"
                        },
                        {
                            "kind": "block",
                            "type": "logic_null"
                        },
                        {
                            "kind": "block",
                            "type": "logic_ternary"
                        }
                    ]
                }
            ]
        },
        {
            "kind": "category",
            "name": "Циклы",
            "categorystyle": "loop_category",
            "contents": [
                {
                    "kind": "block",
                    "type": "controls_repeat_ext",
                    "inputs": {
                        "TIMES": {
                            "block": {
                                "type": "math_number",
                                "fields": {
                                    "NUM": 10
                                }
                            }
                        }
                    }
                },
                {
                    "kind": "block",
                    "type": "controls_whileUntil"
                },
                {
                    "kind": "block",
                    "type": "controls_for",
                    "fields": {
                        "VAR": "i"
                    },
                    "inputs": {
                        "FROM": {
                            "block": {
                                "type": "math_number",
                                "fields": {
                                    "NUM": 1
                                }
                            }
                        },
                        "TO": {
                            "block": {
                                "type": "math_number",
                                "fields": {
                                    "NUM": 10
                                }
                            }
                        },
                        "BY": {
                            "block": {
                                "type": "math_number",
                                "fields": {
                                    "NUM": 1
                                }
                            }
                        }
                    }
                },
                {
                    "kind": "block",
                    "type": "controls_forEach"
                },
                {
                    "kind": "block",
                    "type": "controls_flow_statements"
                }
            ]
        },
        {
            "kind": "category",
            "name": "Математика",
            "categorystyle": "math_category",
            "contents": [
                {
                    "kind": "block",
                    "type": "math_number",
                    "fields": {
                        "NUM": 123
                    }
                },
                {
                    "kind": "block",
                    "type": "math_arithmetic",
                    "fields": {
                        "OP": "ADD"
                    }
                },
                {
                    "kind": "block",
                    "type": "math_single",
                    "fields": {
                        "OP": "ROOT"
                    }
                },
                {
                    "kind": "block",
                    "type": "math_trig",
                    "fields": {
                        "OP": "SIN"
                    }
                },
                {
                    "kind": "block",
                    "type": "math_constant",
                    "fields": {
                        "CONSTANT": "PI"
                    }
                },
                {
                    "kind": "block",
                    "type": "math_number_property",
                    "extraState": "<mutation divisor_input=\"false\"></mutation>",
                    "fields": {
                        "PROPERTY": "EVEN"
                    }
                },
                {
                    "kind": "block",
                    "type": "math_round",
                    "fields": {
                        "OP": "ROUND"
                    }
                },
                {
                    "kind": "block",
                    "type": "math_on_list",
                    "extraState": "<mutation op=\"SUM\"></mutation>",
                    "fields": {
                        "OP": "SUM"
                    }
                },
                {
                    "kind": "block",
                    "type": "math_modulo"
                },
                {
                    "kind": "block",
                    "type": "pow_mod"
                },
                {
                    "kind": "block",
                    "type": "gcd"
                },
                {
                    "kind": "block",
                    "type": "factorint"
                },
                {
                    "kind": "block",
                    "type": "isqrt"
                },
                {
                    "kind": "block",
                    "type": "math_constrain",
                    "inputs": {
                        "LOW": {
                            "block": {
                                "type": "math_number",
                                "fields": {
                                    "NUM": 1
                                }
                            }
                        },
                        "HIGH": {
                            "block": {
                                "type": "math_number",
                                "fields": {
                                    "NUM": 100
                                }
                            }
                        }
                    }
                },
                {
                    "kind": "block",
                    "type": "math_random_int",
                    "inputs": {
                        "FROM": {
                            "block": {
                                "type": "math_number",
                                "fields": {
                                    "NUM": 1
                                }
                            }
                        },
                        "TO": {
                            "block": {
                                "type": "math_number",
                                "fields": {
                                    "NUM": 100
                                }
                            }
                        }
                    }
                },
                {
                    "kind": "block",
                    "type": "math_random_float"
                },
                {
                    "kind": "block",
                    "type": "getrandbits_4_3"
                },
                {
                    "kind": "block",
                    "type": "math_atan2"
                },
                {
                    "kind": "block",
                    "type": "crc32"
                },
                {
                    "kind": "block",
                    "type": "to_int"
                },
                {
                    "kind": "block",
                    "type": "minimum"
                },
                {
                    "kind": "block",
                    "type": "abs"
                },
                {
                    "kind": "block",
                    "type": "decryption_defs"
                },
                {
                    "kind": "block",
                    "type": "logarithm"
                }
            ]
        },
        {
            "kind": "category",
            "name": "Списки",
            "categorystyle": "list_category",
            "contents": [
                {
                    "kind": "block",
                    "type": "decryption_defs_list"
                },
                {
                    "kind": "block",
                    "type": "lists_create_empty"
                },
                {
                    "kind": "block",
                    "type": "lists_create_with",
                    "extraState": {
                        "itemCount": 3
                    }
                },
                {
                    "kind": "block",
                    "type": "lists_repeat",
                    "inputs": {
                        "NUM": {
                            "block": {
                                "type": "math_number",
                                "fields": {
                                    "NUM": 5
                                }
                            }
                        }
                    }
                },
                {
                    "kind": "block",
                    "type": "lists_length"
                },
                {
                    "kind": "block",
                    "type": "lists_isEmpty"
                },
                {
                    "kind": "block",
                    "type": "lists_indexOf",
                    "fields": {
                        "END": "FIRST"
                    }
                },
                {
                    "kind": "block",
                    "type": "lists_getIndex",
                    "fields": {
                        "MODE": "GET",
                        "WHERE": "FROM_START"
                    }
                },
                {
                    "kind": "block",
                    "type": "lists_setIndex",
                    "fields": {
                        "MODE": "SET",
                        "WHERE": "FROM_START"
                    }
                }
            ]
        },
        {
            "kind": "category",
            "name": "Словари",
            "categorystyle": "list_category",
            "contents": [
                {
                    "kind": "block",
                    "type": "dict_create"
                },
                {
                    "kind": "block",
                    "type": "dict_get"
                },
                {
                    "kind": "block",
                    "type": "dict_get_or_default"
                },
                {
                    "kind": "block",
                    "type": "dict_set"
                },
                {
                    "kind": "block",
                    "type": "dict_keys"
                },
                {
                    "kind": "block",
                    "type": "dict_values"
                }
            ]
        },
        {
            "kind": "sep"
        },
        {
            "kind": "category",
            "name": "Переменные",
            "categorystyle": "variable_category",
            "custom": "VARIABLE"
        },
        {
            "kind": "category",
            "name": "Функции",
            "categorystyle": "procedure_category",
            "custom": "PROCEDURE"
        }
    ]
};