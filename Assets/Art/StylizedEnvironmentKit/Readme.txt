//--------------------------------------------
//LMArt - Stylized Environment Kit / Conifer Field
//2022 LittleMarsh CG ART
//version 1.1.0
//The shader is updated to be compatible with point lights and spot lights.
//--------------------------------------------

It is a field composed of conifers and several plants.
It can be used to create a stylized environment.
All screenshots do not use post-processing effects.

Settings
ProjectSettings>Quality
-ShadowDistance: 150
-ShadowCascades: FourCascades


1) Demo Scene
•Showcase
You can see a list of trees and plants that can be used.
/Assets/LMArt-StylizedEnvironmentKit/ConiferField/Scenes/Showcase

•ConiferField
This is a sample scene where trees and plants are actually placed in the field.
/Assets/LMArt-StylizedEnvironmentKit/ConiferField/Scenes/ConiferField

2) Shader Details
•VertexBlending
Textures are blended using the vertex colors red, green, and blue.
The texture's alpha channel (which stores the height map) is used for blending.
/shader/LMArtShader/VertexBlending

•NatureLeaves
It is used for plant leaves.
Built-in wind animation.
You can set the weight of the wind animation with the red vertex color.
It is a double-sided drawing.
/shader/LMArtShader/NatureLeaves

Both are updated to  be compatible with point lights and spot lights.


3) Rendering
Built-In Render Pipeline
Forward rendering
gamma color space


4) Models and Textures
Polycount
-Trees: 2520~6526 Tris
-Plants: 18~512 Tris

Texture resolution
-Ground: 1024x1024
-Plants: 1024x1024
-Leaves: 512x512
-Tree bark: 1024x1024
-Skybox: 12288x2048


5) License
Content may be used for personal and commercial use.
No permission needed.
You are not permitted to sell or distribute any Content (modified or not) by themselves or in a texture pack, material, shader, model.


▽Version
1.0.0
-First release



------------------------------------------------
針葉樹と幾つかの植物で構成されたフィールドです。
フィールドを構成するための素材として活用できます。
すべてのスクリーンショットには、post-processingエフェクトを使用していません。

設定について
ProjectSettings>Quality
-ShadowDistance: 150
-ShadowCascades: FourCascades


1) デモシーン
-Showcase
使用できる木や植物の一覧が確認できます。

-ConiferField
実際に木や植物をフィールドに配置したサンプルシーンです。


2) シェーダーの詳細
-Vertex3Blend
頂点カラーの赤、緑、青、を使用してテクスチャをブレンドしています。
テクスチャのアルファチャンネル(高さマップを格納)はブレンド用に使用します。

-NatureLeaves
植物の葉などに使用するシェーダーです。
風のアニメーションが組み込まれています。
頂点カラーの赤で風のアニメーションのウエイトを設定できます。
両面描画になっています。

どちらのシェーダーもポイントライト、スポットライトに対応できるように更新されています。


3) レンダリングについて
Built-In Render Pipeline
Forward rendering
gamma color space


4) モデルとテクスチャ
ポリゴン数
-Trees: 2520~6526 Tris
-Plants: 18~512 Tris

テクスチャ解像度
-Ground: 1024x1024
-Plants: 1024x1024
-Leaves: 512x512
-Tree bark: 1024x1024
-Skybox: 12288x2048


5) ライセンス
コンテンツは商用利用も可能です。
ライセンス表記は必要ありません。
だたし、コンテンツ(変更されているかどうかに関係なく)を単独で、またはテクスチャパック、マテリアル、シェーダー、モデルで販売または配布することは許可されていません。


▽Version
1.1.0
2022.06















