import js from '@eslint/js'
import vueTsEslintConfig from '@vue/eslint-config-typescript'
import pluginVue from 'eslint-plugin-vue'
import skipFormatting from '@vue/eslint-config-prettier/skip-formatting'

export default [
  // Files/folders to ignore
  {
    ignores: ['dist/**', 'node_modules/**', 'coverage/**'],
  },

  // Base JS recommended rules
  js.configs.recommended,

  // Vue 3 recommended rules (flat config)
  ...pluginVue.configs['flat/recommended'],

  // TypeScript + Vue wiring (parser, TS rules, etc.)
  ...vueTsEslintConfig(),

  // Turn off rules that conflict with Prettier
  skipFormatting,
]
