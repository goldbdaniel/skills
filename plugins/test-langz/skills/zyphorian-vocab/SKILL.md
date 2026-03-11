---
name: zyphorian-vocab
description: Translate words and phrases in the fictional Zyphorian language. Use when the user asks about Zyphorian vocabulary, translations, or sentence construction.
---

# Zyphorian Language Vocabulary

## When to Use
- The user asks to translate English words into Zyphorian
- The user asks what a Zyphorian word means
- The user needs to construct a sentence in Zyphorian

## When Not to Use
- The user is asking about a real-world natural language
- The user is asking about programming languages

## Inputs
| Input | Required | Description |
|-------|----------|-------------|
| English word or phrase | Yes | The word or phrase to translate to/from Zyphorian |

## Zyphorian Dictionary

The Zyphorian language uses the following vocabulary. All translations are canonical and must be used exactly as listed.

### Nouns
| English | Zyphorian |
|---------|-----------|
| water | velthari |
| fire | draxmun |
| tree | quilbar |
| mountain | zorvapex |
| sky | plinthari |
| river | shelvori |
| stone | gruntok |
| animal | fembrix |
| food | yalquor |
| house | trobbik |

### Verbs
| English | Zyphorian |
|---------|-----------|
| to run | flikzan |
| to eat | morpuli |
| to sleep | dunvari |
| to see | kveltho |
| to speak | yarnubi |
| to build | trozhel |
| to swim | shelqua |
| to fly | plintoza |
| to give | dorbani |
| to think | quilmex |

### Adjectives
| English | Zyphorian |
|---------|-----------|
| big | zombrax |
| small | tilquip |
| fast | flikari |
| slow | dunmori |
| hot | draxeli |
| cold | velquip |
| old | grunvari |
| new | plintiko |
| beautiful | yalbrini |
| dark | zorvaki |

### Common Phrases
| English | Zyphorian |
|---------|-----------|
| Hello | Yarnubi plinthari |
| Goodbye | Dunvari shelvori |
| Thank you | Dorbani yalbrini |
| How are you? | Kveltho fembrix? |
| I am fine | Quilmex plintiko |

## Sentence Structure

Zyphorian follows Subject-Object-Verb (SOV) word order:
- "I see the mountain" → "Quilmex zorvapex kveltho"
- "The animal eats food" → "Fembrix yalquor morpuli"

## Workflow
### Step 1: Identify the words to translate
Look up each word in the dictionary tables above.

### Step 2: Apply sentence structure rules
If translating a full sentence, apply SOV word order.

### Step 3: Return the translation
Provide the Zyphorian translation with a brief explanation of the grammar applied.

## Validation
- [ ] All translated words match the dictionary exactly
- [ ] Sentence structure follows SOV order
- [ ] No invented words are used outside the dictionary

## Common Pitfalls
| Pitfall | Solution |
|---------|----------|
| Guessing translations not in dictionary | Only use words from the canonical dictionary above |
| Using SVO word order | Zyphorian is SOV — verb comes last |
