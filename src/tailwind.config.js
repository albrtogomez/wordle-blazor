module.exports = {
    content: ["./**/*.{razor,razor.cs,html}"],
    theme: {
        colors: {
            transparent: 'transparent',
            current: 'currentColor',
            'white': '#ffffff',
            'black': '#000000',
            'lightgray': '#d3d3d3',
            'middlegray': '#d3d6da',
            'darkgray': '#787c7e',
            'darkgrayhover': '#aeb0b2',
            'green': '#6aaa64',
            'greenhover': '#a6cca2',
            'yellow': '#c9b458',
            'yellowhover': '#dfd29b',
            'keyhover': '#e5e6e9',
            'keyactive': '#e0e6ec',
            'keyactiveborder': '#bbdaf4',
            'semitransparent': '#333',
            'closehover': '#f0f0f0',
            'whitetransparent': '#ffffff99',
            'ultralightgray': '#edeff1',
        },
        extend: {
            transitionProperty: {
                'keyactive': 'background-color, border-color',
            },
            animation: {
                'fade': 'fadein 0.5s, fadeout 0.5s 2.5s',
                'shake': 'shake 0.82s cubic-bezier(.36,.07,.19,.97) both'
            },
            keyframes: {
                fadein: {
                    from: { opacity: 0 },
                    to: { opacity: 1 },
                },
                fadeout: {
                    from: { opacity: 1 },
                    to: { opacity: 0 },
                },
                shake: {
                    '10%, 90%': {
                        transform: 'translate3d(-1px, 0, 0)'
                    },
                    '20%, 80%': {
                        transform: 'translate3d(2px, 0, 0)'
                    },
                    '30%, 50%, 70%': {
                        transform: 'translate3d(-4px, 0, 0)'
                    },

                    '40%, 60%': {
                        transform: 'translate3d(4px, 0, 0)'
                    },
                }
            },
        },
    },
    plugins: [],
}